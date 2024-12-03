using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myeongjin
{
	public class Singleton<T> : MonoBehaviour where T : Component
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindAnyObjectByType<T>();
					if (_instance == null)
					{
						GameObject obj = new GameObject();
						obj.name = typeof(T).Name;
						_instance = obj.AddComponent<T>();
					}
				}
				return _instance;
			}
		}
		public virtual void Awake()
		{
			if (_instance == null)
			{
				_instance = this as T;
				DontDestroyOnLoad(gameObject);
			}
			else
				Destroy(gameObject);
		}
	}
}