using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CProcessorAssembler.Commands
{
    internal class LDDA : Command
    {
        public LDDA(string comment) : base(ECommand.LDDA, comment) { }
    }

    internal class LDDB : Command
    {
        public LDDB(string comment) : base(ECommand.LDDB, comment) { }
    }

    internal class STDA : Command
    {
        public STDA(string comment) : base(ECommand.STDA, comment) { }
    }

    internal class STDB : Command
    {
        public STDB(string comment) : base(ECommand.STDB, comment) { }
    }

    internal class ADDA : Command
    {
        public ADDA(string comment) : base(ECommand.ADDA, comment) { }
    }

    internal class SUBA : Command
    {
        public SUBA(string comment) : base(ECommand.SUBA, comment) { }
    }

    internal class ANDA : Command
    {
        public ANDA(string comment) : base(ECommand.ANDA, comment) { }
    }

    internal class ORA : Command
    {
        public ORA(string comment) : base(ECommand.ORA, comment) { }
    }

    internal class NOTA : Command
    {
        public NOTA(string comment) : base(ECommand.NOTA, comment) { }
    }

    internal class INCA : Command
    {
        public INCA(string comment) : base(ECommand.INCA, comment) { }
    }

    internal class DECA : Command
    {
        public DECA(string comment) : base(ECommand.DECA, comment) { }
    }

    internal class ADDB : Command
    {
        public ADDB(string comment) : base(ECommand.ADDB, comment) { }
    }

    internal class SUBB : Command
    {
        public SUBB(string comment) : base(ECommand.SUBB, comment) { }
    }

    internal class ANDB : Command
    {
        public ANDB(string comment) : base(ECommand.ANDB, comment) { }
    }

    internal class ORB : Command
    {
        public ORB(string comment) : base(ECommand.ORB, comment) { }
    }

    internal class NOTB : Command
    {
        public NOTB(string comment) : base(ECommand.NOTB, comment) { }
    }

    internal class INCB : Command
    {
        public INCB(string comment) : base(ECommand.INCB, comment) { }
    }

    internal class DECB : Command
    {
        public DECB(string comment) : base(ECommand.DECB, comment) { }
    }

    internal class CMP : Command
    {
        public CMP(string comment) : base(ECommand.CMP, comment) { }
    }

    internal class NOP : Command
    {
        public NOP(string comment) : base(ECommand.NOP, comment) { }
    }
}
