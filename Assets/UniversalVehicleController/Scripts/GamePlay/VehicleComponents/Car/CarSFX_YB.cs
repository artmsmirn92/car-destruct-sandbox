// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace PG
// {
//     /// <summary>
//     /// Sound effects, using FMOD.
//     /// </summary>
//     public class CarSFX_YB : VehicleSFX
//     {
//         #region constants
//
//         [SerializeField] private AudioYB EngineSourceRef;
//
//         #endregion
//         
//
//
//         [SerializeField] private string StartEngineClip;
//         [SerializeField] private string StopEngineClip;
//         [SerializeField] private string LowEngineClip;
//         [SerializeField] private string MediumEngineClip;
//         [SerializeField] private string HighEngineClip;
//
//         [SerializeField] private float MinEnginePitch = 0.5f;
//         [SerializeField] private float MaxEnginePitch = 1.5f;
//
//         [Header("Additional settings")]
//         [SerializeField]
//         private AudioYB TurboSource;
//         [SerializeField] private string TurboBlowOffClip;
//         [SerializeField] private float  MaxBlowOffVolume            = 0.5f;
//         [SerializeField] private float  MinTimeBetweenBlowOffSounds = 1;
//         [SerializeField] private float  MaxTurboVolume              = 0.5f;
//         [SerializeField] private float  MinTurboPith                = 0.5f;
//         [SerializeField] private float  MaxTurboPith                = 1.5f;
//
//         [SerializeField] private AudioYB BoostSource;
//
//         [SerializeField] private List<string> BackFireClips;
//
//         [Header ("Wind Sound")]
//         [SerializeField]
//         private AudioYB SpeedWindSource;
//         [SerializeField] private float WindSoundStartSpeed = 20;
//         [SerializeField] private float WindSoundMaxSpeed   = 100;
//         [SerializeField] private float WindSoundStartPitch = 0.4f;
//         [SerializeField] private float WindSoundMaxPitch   = 1.5f;
//
//         
//
//         private CarController     Car;
//         private float             LastBlowOffTime;
//         private float[]           EngineSourcesRanges = new float[1] { 1f };
//         private List<AudioYB> EngineSources       = new List<AudioYB>();
//
//         protected override void Start ()
//         {
//             base.Start ();
//
//             Car = Vehicle as CarController;
//
//             if (Car == null)
//             {
//                 Debug.LogErrorFormat ("[{0}] CarSFX without CarController in parent", name);
//                 enabled = false;
//                 return;
//             }
//
//             if (BoostSource)
//             {
//                 if (Car.Engine.EnableBoost && BoostSource.gameObject.activeInHierarchy)
//                 {
//                     UpdateAction += UpdateBoost;
//                 }
//                 else
//                 {
//                     BoostSource.Stop ();
//                 }
//             }
//
//             if (TurboSource)
//             {
//                 if (Car.Engine.EnableTurbo && TurboSource.gameObject.activeInHierarchy)
//                 {
//                     UpdateAction += UpdateTurbo;
//                 }
//                 else
//                 {
//                     TurboSource.Stop ();
//                 }
//             }
//
//             Car.BackFireAction += OnBackFire;
//             Car.OnStartEngineAction += StartEngine;
//             Car.OnStopEngineAction += StopEngine;
//
//             if (EngineSourceRef && EngineSourceRef.gameObject.activeInHierarchy)
//             {
//
//                 //Create engine sounds list.
//                 var engineClips = new List<string>();
//                 if (LowEngineClip != null)
//                     engineClips.Add (LowEngineClip);
//                 if (MediumEngineClip != null)
//                     engineClips.Add (MediumEngineClip);
//                 if (HighEngineClip != null)
//                     engineClips.Add (HighEngineClip);
//
//                 EngineSourcesRanges = engineClips.Count switch
//                 {
//                     2 => new[] {0.3f, 1f},
//                     3 => new[] {0.3f, 0.6f, 1f},
//                     _ => EngineSourcesRanges
//                 };
//
//                 //Init Engine sounds.
//                 if (engineClips.Count > 0)
//                 {
//                     AudioYB engineSource;
//
//                     for (int i = 0; i < engineClips.Count; i++)
//                     {
//                         if (EngineSourceRef.Clip == engineClips[i])
//                         {
//                             engineSource = EngineSourceRef;
//                             EngineSourceRef.transform.SetSiblingIndex (EngineSourceRef.transform.parent.childCount);
//                         }
//                         else
//                         {
//                             engineSource = Instantiate (EngineSourceRef, EngineSourceRef.transform.parent);
//                             engineSource.Clip = engineClips[i];
//                             engineSource.Play ();
//                         }
//
//                         engineSource.name = $"Engine source ({i})";
//                         EngineSources.Add (engineSource);
//                     }
//
//                     if (!EngineSources.Contains (EngineSourceRef))
//                     {
//                         Destroy (EngineSourceRef);
//                     }
//                 }
//
//                 if (!Car.EngineIsOn)
//                 {
//                     if (EngineSources is {Count: > 0})
//                         EngineSources.ForEach (_S => _S.Pitch = 0);
//                     else
//                         EngineSourceRef.Pitch = 0;
//                 }
//
//                 UpdateAction += UpdateEngine;
//             }
//
//             if (SpeedWindSource && SpeedWindSource.gameObject.activeInHierarchy)
//             {
//                 UpdateAction += UpdateWindEffect;
//             }
//
//         }
//
//         private void StartEngine (float _StartDellay)
//         {
//             if (StartEngineClip != null)
//                 OtherEffectsSource.PlayOneShot (StartEngineClip);
//         }
//
//         private void StopEngine ()
//         {
//             if (StopEngineClip != null)
//                 OtherEffectsSource.PlayOneShot (StartEngineClip);
//         }
//
//         //Base engine sounds
//         private void UpdateEngine ()
//         {
//             if (Car.EngineIsOn)
//             {
//                 if (EngineSources.Count == 0 && EngineSourceRef && EngineSourceRef.gameObject.activeInHierarchy)
//                 {
//                     EngineSourceRef.Pitch = Mathf.Lerp (MinEnginePitch, MaxEnginePitch, (Car.EngineRPM - Car.MinRPM) / (Car.MaxRPM - Car.MinRPM));
//                 }
//                 else if (EngineSources.Count > 1)
//                 {
//                     float rpmNorm = ((Car.EngineRPM - Car.MinRPM) / (Car.MaxRPM - Car.MinRPM)).Clamp();
//                     float pith = Mathf.Lerp (MinEnginePitch, MaxEnginePitch, rpmNorm);
//
//                     for (int i = 0; i < EngineSources.Count; i++)
//                     {
//                         EngineSources[i].Pitch = pith;
//
//                         if (i > 0 && rpmNorm < EngineSourcesRanges[i - 1])
//                         {
//                             EngineSources[i].Volume = Mathf.InverseLerp (0.2f, 0, EngineSourcesRanges[i - 1] - rpmNorm);
//                         }
//                         else if (rpmNorm > EngineSourcesRanges[i])
//                         {
//                             EngineSources[i].Volume = Mathf.InverseLerp (0.3f, 0, rpmNorm - EngineSourcesRanges[i]);
//                         }
//                         else
//                         {
//                             EngineSources[i].Volume = 1;
//                         }
//
//                         if (Mathf.Approximately (EngineSources[i].Volume, 0) && EngineSources[i].IsPlaying)
//                         {
//                             EngineSources[i].Stop ();
//                         }
//
//                         if (EngineSources[i].Volume > 0 && !EngineSources[i].IsPlaying)
//                         {
//                             EngineSources[i].Play ();
//                         }
//                     }
//                 }
//             }
//             else //if (!EngineIsON)
//             {
//                 float pith = Mathf.Lerp (0, MinEnginePitch, (Car.EngineRPM / Car.MinRPM).Clamp());
//                 if (EngineSources.Count == 0 && EngineSourceRef && EngineSourceRef.gameObject.activeInHierarchy)
//                 {
//                     EngineSourceRef.Pitch = Mathf.MoveTowards(EngineSourceRef.Pitch, pith, Time.deltaTime);
//                 }
//                 else if (EngineSources.Count > 1)
//                 {
//                     EngineSources[0].Pitch = Mathf.MoveTowards (EngineSources[0].Pitch, pith, Time.deltaTime);
//                     for (int i = 1; i < EngineSources.Count; i++)
//                     {
//                         EngineSources[i].Pitch = pith;
//                         if (EngineSources[i].IsPlaying)
//                         {
//                             EngineSources[i].Stop ();
//                         }
//                     }
//                 }
//             }
//         }
//
//         //Additional turbo sound
//         private void UpdateTurbo ()
//         {
//             if (!Car.Engine.EnableTurbo || !TurboSource || !TurboSource.gameObject.activeInHierarchy)
//                 return;
//             TurboSource.Volume = Mathf.Lerp (0, MaxTurboVolume, Car.CurrentTurbo);
//             TurboSource.Pitch = Mathf.Lerp (MinTurboPith, MaxTurboPith, Car.CurrentTurbo);
//             if (!(Car.CurrentTurbo > 0.2f) || (!(Car.CurrentAcceleration < 0.2f) && !Car.InChangeGear) ||
//                 (!((Time.realtimeSinceStartup - LastBlowOffTime) > MinTimeBetweenBlowOffSounds)))
//                 return;
//             OtherEffectsSource.PlayOneShot (TurboBlowOffClip, Car.CurrentTurbo * MaxBlowOffVolume);
//             LastBlowOffTime = Time.realtimeSinceStartup;
//         }
//
//         //Additional boost sound
//         private void UpdateBoost ()
//         {
//             if (!Car.Engine.EnableBoost || !BoostSource || !BoostSource.gameObject.activeInHierarchy) 
//                 return;
//             switch (Car.InBoost)
//             {
//                 case true when !BoostSource.IsPlaying:
//                     BoostSource.Play ();
//                     break;
//                 case false when BoostSource.IsPlaying:
//                     BoostSource.Stop ();
//                     break;
//             }
//         }
//
//         private void UpdateWindEffect ()
//         {
//             if (Car.IsPlayerVehicle)
//             {
//                 float currentSpeedNorm = Mathf.InverseLerp (WindSoundStartSpeed, WindSoundMaxSpeed, Car.CurrentSpeed);
//                 if (currentSpeedNorm > 0 && !SpeedWindSource.IsPlaying)
//                 {
//                     SpeedWindSource.Play ();
//                 }
//                 SpeedWindSource.Volume = currentSpeedNorm;
//                 SpeedWindSource.Pitch = Mathf.Lerp (WindSoundStartPitch, WindSoundMaxPitch, currentSpeedNorm);
//             }
//             else if (SpeedWindSource.IsPlaying)
//             {
//                 SpeedWindSource.Stop ();
//             }
//         }
//
//         private void OnBackFire ()
//         {
//             if (BackFireClips is {Count: > 0})
//                 OtherEffectsSource.PlayOneShot (BackFireClips[Random.Range (0, BackFireClips.Count - 1)]);
//         }
//     }
// }
