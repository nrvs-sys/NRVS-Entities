using FishNet;
using FishNet.Managing.Timing;
using FishNet.Object;
using Core;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using FishNet.Managing.Predicting;

namespace Core
{
	public sealed class UpdateEvent : NetworkComponent
	{
		public UpdateType updateType;

		public UnityEvent<float> onUpdate;

		private TimeManager timeManager;

		private PredictionManager predictionManager;

		public override void OnStartNetwork()
		{
			base.OnStartNetwork();

			timeManager = InstanceFinder.TimeManager;
			if (timeManager != null)
			{
				timeManager.OnPreTick += TimeManager_OnPreTick;
				timeManager.OnTick += TimeManager_OnTick;
				timeManager.OnPostTick += TimeManager_OnPostTick;
			}

			predictionManager = InstanceFinder.PredictionManager;
			if (predictionManager != null)
			{
                predictionManager.OnPreReconcile += PredictionManager_OnPreReconcile;
                predictionManager.OnPostReconcile += PredictionManager_OnPostReconcile;
			}
		}

		public override void OnStopNetwork()
		{
			base.OnStopNetwork();

			if (timeManager != null)
			{
				timeManager.OnPreTick -= TimeManager_OnPreTick;
				timeManager.OnTick -= TimeManager_OnTick;
				timeManager.OnPostTick -= TimeManager_OnPostTick;
			}

            predictionManager = InstanceFinder.PredictionManager;
            if (predictionManager != null)
            {
                predictionManager.OnPreReconcile -= PredictionManager_OnPreReconcile;
                predictionManager.OnPostReconcile -= PredictionManager_OnPostReconcile;
            }
        }

		private void Update()
		{
			if (updateType.HasFlag(UpdateType.Update))
				Sync(Time.deltaTime);
		}

		private void FixedUpdate()
		{
			if (updateType.HasFlag(UpdateType.FixedUpdate))
				Sync(Time.fixedDeltaTime);
		}

		private void LateUpdate()
		{
			if (updateType.HasFlag(UpdateType.LateUpdate))
				Sync(Time.deltaTime);
		}

		private void TimeManager_OnPreTick()
		{
			if (updateType.HasFlag(UpdateType.PreTick))
				Sync((float)timeManager.TickDelta);
		}

		private void TimeManager_OnTick()
		{
			if (updateType.HasFlag(UpdateType.Tick))
				Sync((float)timeManager.TickDelta);
		}

		private void TimeManager_OnPostTick()
		{
			if (updateType.HasFlag(UpdateType.PostTick))
				Sync((float)timeManager.TickDelta);
		}
		private void PredictionManager_OnPreReconcile(uint clientTick, uint serverTick)
		{
			if (updateType.HasFlag(UpdateType.PreReconcile))
				Sync((float)timeManager.TickDelta);
		}

		private void PredictionManager_OnPostReconcile(uint clientTick, uint serverTick)
		{
			if (updateType.HasFlag(UpdateType.PostReconcile))
				Sync((float)timeManager.TickDelta);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Sync(float dt)
		{
			onUpdate?.Invoke(dt);
		}
	}
}
