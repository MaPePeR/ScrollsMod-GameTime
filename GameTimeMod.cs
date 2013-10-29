using System;
using ScrollsModLoader.Interfaces;
using Mono.Cecil;
using UnityEngine;
using System.Reflection;
namespace GameTimeMod
{
	public class GameTimeMod : BaseMod, ICommListener
	{
		DateTime gameStarted = DateTime.Now;
		FieldInfo guiClockGUISkin;
		public GameTimeMod () {
			guiClockGUISkin = typeof(DeckBuilder2).Assembly.GetType ("GUIClock").GetField ("guiSkin", BindingFlags.Instance | BindingFlags.NonPublic);
			App.Communicator.addListener (this);
		}

		public static MethodDefinition[] GetHooks(TypeDefinitionCollection scrollsTypes, int version) {
			MethodDefinition[] method;
			method = scrollsTypes ["GUIClock"].Methods.GetMethod("renderTime");
			if (method.Length == 1) {
				return method;
			}
			return new MethodDefinition[] {};
		}

		public static string GetName() {
			return "GameTime";
		}
		public static int GetVersion() {
			return 2;
		}

		public override void BeforeInvoke (InvocationInfo info) {
		}

		public override void AfterInvoke (InvocationInfo info, ref object returnValue) {
			GUISkin skin = GUI.skin;
			GUI.skin = (GUISkin)guiClockGUISkin.GetValue (info.target);
			Rect rect = new Rect (
				(float)((double)Screen.width * 0.5 - (double)Screen.height * 0.0599999986588955), 
				(float)Screen.height * (0.09f - 0.035f), 
				(float)Screen.height * 0.12f, 
				(float)Screen.height * 0.035f
			);
			int fontSize = GUI.skin.label.fontSize;
			GUI.skin.label.fontSize = Screen.height / 45;
			TimeSpan gamelength = DateTime.Now.Subtract (gameStarted);
			GUI.Label (rect, timeSpanToString(gamelength));
			GUI.skin.label.fontSize = fontSize;
			GUI.skin = skin;
		}

		public void handleMessage (Message msg){
			if (msg is GameInfoMessage) {
				gameStarted = DateTime.Now;
			}
		}

		public void onReconnect () {}

		public static string timeSpanToString(TimeSpan t) {
			if (t.Hours > 0) {
				return String.Format("{0:d}:{1:d2}:{2:d2}", (int)t.TotalHours, t.Minutes, t.Seconds);
			} else {
				return String.Format("{0:d}:{1:d2}", (int)t.TotalMinutes, t.Seconds);
			}
		}

		public void onConnect (OnConnectData data) {
		}
	}
}

