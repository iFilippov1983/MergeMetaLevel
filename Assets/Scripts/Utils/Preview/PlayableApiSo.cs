using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

public class PlayableApiSo : ScriptableObject
{
    public PlayableDirector Director;
    public PlayableAsset Asset;
    private bool _isReady;

    [Button]
    public async void Play()
    {
        Debug.Log("START");
        Director.stopped += SetReady; 
        Director.Stop();
        Director.Play(Asset);
        var graph = Director.playableGraph;
        while ( graph.IsValid() && !graph.IsDone() )
            await Task.Yield();
        
        Director.stopped -= SetReady; 
        Debug.Log("END");
    }

    private void SetReady(PlayableDirector director)
    {
        Debug.Log("ENDEEEC");
    }
}