﻿#if NETFULL || NETCOREAPP3_1 || NET5_0
namespace JavaScriptEngineSwitcher.Tests.V8
{
	public class Es2015Tests : Es2015TestsBase
	{
		protected override string EngineName
		{
			get { return "V8JsEngine"; }
		}
	}
}
#endif