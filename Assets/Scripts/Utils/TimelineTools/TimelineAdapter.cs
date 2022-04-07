using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Utils.TimelineTools
{
    public class TimelineAdapter
    {
        private readonly PlayableDirector _director;
        private readonly PlayableAsset _asset;
        private readonly Dictionary<string,PlayableBinding> _bindings;
        private readonly Dictionary<(string, string),PlayableAsset> _clips;
        private readonly string _name;

        public TimelineAdapter(PlayableDirector director, PlayableAsset asset)
        {
            // _director.<>()  
            _name = _director.gameObject.name;
            _director = director;
            _asset = asset;
            _bindings = new Dictionary<string, PlayableBinding>();
            _clips = new Dictionary<(string, string), PlayableAsset>();

            foreach (var playableBinding in _asset.outputs)
            {
                var trackName = playableBinding.streamName;
                _bindings.Add(trackName, playableBinding);

                var track = playableBinding.sourceObject as TrackAsset;
                foreach (var clip in track.GetClips())
                {
                    var clipName = clip.displayName;
                    _clips.Add((trackName, clipName), clip.asset as PlayableAsset);
                }
            }
        }

        public void SetBinding(string trackName, Object obj)
        {
            var playableBinding = _bindings.SaveGet(trackName);
            Log.NullWarning(playableBinding, $"PlayableDirector {_name} track not found {trackName}");
            _director.SetGenericBinding(playableBinding.sourceObject, obj);
        }

        public T GetTrack<T>(string trackName) where  T: TrackAsset
        {
            var playableBinding = _bindings[trackName];
            Log.NullWarning(playableBinding, $"PlayableDirector {_name} track not found {trackName}");
            return playableBinding.sourceObject as T;
        }

        public T GetClip<T>(string trackName, string clipName) where T : PlayableAsset
        {
            var clip = _clips.SaveGet((trackName, clipName));
            Log.NullWarning(clip, $"PlayableDirector {_name} clip not found  track: {trackName}, clip: {clipName}");
            return clip as T;
        }

        public void Play() 
            => _director.Play();

        public void Pause() 
            => _director.Pause();
        
        public void UnPause() 
            => _director.Resume();
    }
}