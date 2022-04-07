using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinemachine;
using Data;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utils
{
    public static class TimelineExtensions
    {
        public static async Task PlayAsync(this PlayableDirector Director, PlayableAsset Asset)
        {
            Director.Play(Asset);
            var graph = Director.playableGraph;

            while (graph.IsValid() && !graph.IsDone())
                await Task.Yield();
        }

        public static Object GetBinding(this PlayableDirector director, TrackAsset asset)
            => director.GetGenericBinding(asset);

        public static T GetBinding2<T>(this PlayableDirector director, TrackAsset trackAsset) where T : class
        {
            var bindObjectRaw = director.GetGenericBinding(trackAsset);
            var bindObject = bindObjectRaw as T;
            bindObject ??= (bindObjectRaw as GameObject)?.GetComponent<T>();
            bindObject ??= (bindObjectRaw as GameObject)?.GetComponentInChildren<T>();
            return bindObject;
        }
        
        public static async Task Wait(this PlayableDirector Director)
        {
            var graph = Director.playableGraph;
            while ( graph.IsValid() && !graph.IsDone() )
                await Task.Yield();
        }
        
        // NOTE : use it in PlayableBehaviour
        // e.g public override void OnPlayableCreate(Playable playable) _dir = playable.GetDirector();
        public static PlayableDirector GetDirector(Playable playable)
        {
            return playable.GetGraph().GetResolver() as PlayableDirector;
        }
        
        public static void JumpTo(this Playable playable, double time)
        {
            playable.GetGraph().GetRootPlayable(0).SetTime(time);
        }

        public static void Pause(this Playable playable, bool pause)
        {
            playable.GetGraph().GetRootPlayable(0).SetSpeed(pause ? 0 : 1 );
        }
        
        public static double EndOfClipTime(this Playable playable)
        {
            var _graph = playable.GetGraph();
            return _graph.GetRootPlayable(0).GetTime() + playable.GetDuration() ;
        }

        public static bool IsBehaviourOnEnter(this Playable playable, FrameData info)
        {
            var isBehaviourPlayOnEnter = (info.frameId == 0) || (info.deltaTime > 0);
            return isBehaviourPlayOnEnter;
        }

        public static bool IsBehaviourOnExit(this Playable playable, FrameData info)
        {
            var duration = playable.GetDuration();
            var count = playable.GetTime() + info.deltaTime;
            var isBehaviurPauseOnExit = ((info.effectivePlayState == PlayState.Paused && count > duration) ||
                                         playable.GetGraph().GetRootPlayable(0).IsDone());

            return isBehaviurPauseOnExit;
        }

        public static T GetClip<T>(this PlayableDirector director, string trackName, string clipName) where T : PlayableAsset
        {
            var asset = director.playableAsset;
            foreach (var output in asset.outputs)
            {
                var _trackName = output.streamName;
                var trackAsset = output.sourceObject as TrackAsset;
                foreach (var timelineClip in trackAsset.GetClips())
                {
                    var _clipName = timelineClip.displayName;
                    if (_trackName == trackName && _clipName == clipName)
                        return timelineClip.asset as T;
                }
            }

            return null;
        }

        public static void Bind<TTrack, TId>(this PlayableDirector director, List<TId> ids, System.Func<TId, Object> cb)
        {
            var timeline = director.playableAsset as TimelineAsset;
            int i = 0;
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track is TTrack && i < ids.Count)
                {
                    director.SetGenericBinding(track, cb(ids[i]));
                    ++i;
                }
            }
        }
        
        
        public static void Bind<TTrack>(this PlayableDirector director, int maxCount, System.Func<int, Object> cb)
        {
            var timeline = director.playableAsset as TimelineAsset;
            int i = 0;
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track is TTrack && i < maxCount)
                {
                    director.SetGenericBinding(track, cb(i));
                    ++i;
                }
            }
        }

        public static List<T> GetTracks<T>(this PlayableDirector director) where T : class
        {
            var res = new List<T>();
            var timeline = director.playableAsset as TimelineAsset;
            foreach (var track in timeline.GetOutputTracks())
                if (track is T)
                    res.Add(track as T);

            return res;
        }
        public static TrackAsset GetTrack(this PlayableDirector director, string trackName)
        {
            var timeline = director.playableAsset as TimelineAsset;
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == trackName)
                    return track;
            }

            return default;
        }   
        public static IEnumerable<TimelineClip> GetCinemachineClips(this PlayableDirector director, string trackName)
        {
            var timeline = director.playableAsset as TimelineAsset;
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == trackName)
                    return track.GetClips();
            }

            return default;
        }
        
        public static void Bind(this PlayableDirector director, CinemachineTrack track, string clipName, CinemachineVirtualCamera camera)
        {
            foreach (var clip in track.GetClips())
            {
                if (clip.displayName == clipName)
                {
                    var asset = clip.asset as CinemachineShot;
                    if (asset != null)
                        director.SetReferenceValue(asset.VirtualCamera.exposedName, camera);
                }
            }
        }

        public static IEnumerable<TrackAsset> FindTrack(this PlayableDirector director, Func<TrackAsset, bool> cbCondition)
        {
            var timeline = director.playableAsset as TimelineAsset;
            var tracks = timeline
                .GetOutputTracks()
                .Where(t => cbCondition(t));
            return tracks;
        }

        public static void SetBinding(this TrackAsset track, PlayableDirector director, Object obj)
        {
            director.SetGenericBinding(track, obj);
        }

        public static void BindAll(this PlayableDirector director, string trackName, Object obj) 
        {
            var timeline = director.playableAsset as TimelineAsset;
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == trackName)
                    director.SetGenericBinding(track, obj);
            }
        }

        public static void DisplayBindingsOfType<T>(this PlayableDirector director)
        {
            var timeline = director.playableAsset as TimelineAsset;
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track is T)
                {
                    Debug.Log($"{track.name}");
                    // director.SetGenericBinding(track, obj);
                    // break;
                }
            }

        }
        
        public static void DisplayBindings(this PlayableDirector director)
        {
#if UNITY_EDITOR
            var obj = new SerializedObject(director);
            var bindings = obj.FindProperty("m_SceneBindings");
            var res = "";
            for (int i = 0; i < bindings.arraySize; i++)
            {
                var binding = bindings.GetArrayElementAtIndex(i);
                var trackProp = binding.FindPropertyRelative("key");
                var sceneObjProp = binding.FindPropertyRelative("value");
                var objType = binding.FindPropertyRelative("type");
                var track = trackProp.objectReferenceValue;
                var type = trackProp.propertyType;
                var sceneObj = sceneObjProp.objectReferenceValue;

                var trackName = track != null ? track.name : "Null";
                var objName = sceneObj != null ? sceneObj.name : "Null";
                res += $"Binding {trackName} {objName} <<< {objType?.name} \n";
            }

            Debug.Log(res);
#endif
        }

        
    }
}