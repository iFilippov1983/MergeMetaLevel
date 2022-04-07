using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace Mono
{
    public class ProfilerStats : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        
        string statsText;
        ProfilerRecorder systemMemoryRecorder;
        ProfilerRecorder gcMemoryRecorder;
        ProfilerRecorder mainThreadTimeRecorder;
        ProfilerRecorder renderRecorder;
        ProfilerRecorder drawCalls;
        ProfilerRecorder cameraRender;

        void OnEnable()
        {
            systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
            gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
            mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
            renderRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Rendering Time");
            drawCalls = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
            cameraRender = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Camera.Render");
        }


        void OnDisable()
        {
            systemMemoryRecorder.Dispose();
            gcMemoryRecorder.Dispose();
            mainThreadTimeRecorder.Dispose();
            drawCalls.Dispose();
        }

        void Update()
        {
            var sb = new StringBuilder(500);
            sb.AppendLine($"Frame Time: {GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f):F1} ms and its unit type {mainThreadTimeRecorder.UnitType}");
            sb.AppendLine($"Rendering Time: {renderRecorder.LastValue} ");
            sb.AppendLine($"Draw Calls: {drawCalls.LastValue}");
            sb.AppendLine($"GC Memory: {gcMemoryRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"System Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB");
            // sb.AppendLine($"Camera Render: {cameraRender.LastValue} ");
            sb.AppendLine($"Rendering Time: {cameraRender.LastValue * (1e-6f):F1} ms");

            Text.text = sb.ToString();
        }
        
        static double GetRecorderFrameAverage(ProfilerRecorder recorder)
        {
            var samplesCount = recorder.Capacity;
            if (samplesCount == 0)
                return 0;

            double r = 0;
        
            var samples = new List<ProfilerRecorderSample>(samplesCount);
            recorder.CopyTo(samples);
            for (var i = 0; i < samples.Count; ++i)
                r += samples[i].Value;
            r /= samplesCount;

            return r;
        }
    }
}