using System;
using System.Collections.Generic;
using System.Text;

namespace ELEPHANTSRPG.StateManagement
{
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
