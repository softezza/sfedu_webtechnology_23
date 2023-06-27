using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTwin.NoesisGUI.Regions
{
	/// <summary>
	/// Интерфейс контракт на получение контекста региона.
	/// <br>Представление <see cref="SmartTwin.NoesisGUI.Views.BaseView"/> или его контекст при реализации интерфейса будут получать контекст региона при добавлении в сам регион</br>
	/// </summary>
	public interface IRegionContextAccepted
	{
		void AcceptRegionContext(object regionContext);
	}
}
