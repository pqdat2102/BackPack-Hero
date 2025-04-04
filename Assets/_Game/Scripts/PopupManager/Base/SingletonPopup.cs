namespace GamePlugins
{
	public class SingletonPopup<T> : BasePopup where T : BasePopup
	{
		private static T _instance;

		public static T Instance => _instance;

		public override void Awake()
		{
			_instance = this as T;
		}
	}
}
