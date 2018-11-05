namespace IWPCIH.EventTracking
{
	[System.Serializable]
	public struct TimelineEvent
	{
		public enum EventType { CropStart };

		public float InvokeTime;
		public EventType Type;
		public int Id;
	}
}
