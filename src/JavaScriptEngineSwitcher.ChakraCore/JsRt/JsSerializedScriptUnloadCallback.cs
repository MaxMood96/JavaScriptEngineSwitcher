﻿namespace JavaScriptEngineSwitcher.ChakraCore.JsRt
{
	/// <summary>
	/// Called by the runtime when it is finished with all resources related to the script execution.
	/// The caller should free the source if loaded, the byte code, and the context at this time.
	/// </summary>
	/// <param name="sourceContext">The context passed to <c>Js[Parse|Run]SerializedScriptWithCallback</c></param>
	internal delegate void JsSerializedScriptUnloadCallback(JsSourceContext sourceContext);
}