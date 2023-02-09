using Lofelt.NiceVibrations;
using UnityEngine;

public class VibrationController : Singleton<VibrationController>
{
    protected bool _continuousActive = false;
    protected float _amplitudeLastFrame = -1f;
    protected float _frequencyLastFrame = -1f;

    public void Vibrate(HapticPatterns.PresetType hapticType)
    {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
    }

    public virtual void ContinuousHaptics(float ContinuousAmplitude, float ContinuousFrequency, float ContinuousDuration)
    {
        if (!_continuousActive)
        {
            // START
            HapticController.fallbackPreset = HapticPatterns.PresetType.LightImpact;
            HapticPatterns.PlayConstant(ContinuousAmplitude, ContinuousFrequency, ContinuousDuration);

        }
        else
        {
            // STOP
            HapticController.Stop();
        }
    }

    public void StopContinuousHaptic()
    {
        HapticController.Stop();
    }
}
