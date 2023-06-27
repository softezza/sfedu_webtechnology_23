using SmartTwin.NoesisGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace NUIIntergration {

	public class Startup : MonoBehaviour
	{
		NoesisEngine _engine;

		[Inject]
		private void Construct(NoesisEngine engine) => _engine = engine;

		private void Start() => _engine.Initialize();
	}
}