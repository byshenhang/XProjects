﻿using System;

namespace XUtliPoolLib
{

	public enum LoadState
	{

		State_None,

		State_LoadingAsync,

		State_Loading,

		State_Error,

		State_Complete
	}
}
