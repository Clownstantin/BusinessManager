using Leopotam.EcsLite;
using UnityEngine;

public class ECSStartup : MonoBehaviour
{
	EcsWorld _world;
	IEcsSystems _systems;

	void Start()
	{
		// Создаем окружение, подключаем системы.
		_world = new EcsWorld();
		_systems = new EcsSystems(_world);
		_systems.Add(null)
				.Init();
	}

	void Update()
	{
		// Выполняем все подключенные системы.
		_systems?.Run();
	}

	void OnDestroy()
	{
		// Уничтожаем подключенные системы.
		if(_systems != null)
		{
			_systems.Destroy();
			_systems = null;
		}
		// Очищаем окружение.
		if(_world != null)
		{
			_world.Destroy();
			_world = null;
		}
	}
}
