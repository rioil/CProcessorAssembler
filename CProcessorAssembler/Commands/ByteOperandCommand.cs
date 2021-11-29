using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CProcessorAssembler.Commands
{
    internal abstract class ByteOperandCommand : Command
    {
        public ByteOperandCommand(ECommand command, byte operand, string comment) : base(command, comment)
        {
            Operand = operand;
        }

        public byte Operand { get; }

        public override byte[] ToBytes(Dictionary<string, Command> labels, EEndianness endianness = EEndianness.Big)
        {
            return new byte[] { (byte)CommandType, Operand };
        }
    }

    internal class SETIXH : ByteOperandCommand
    {
        public SETIXH(byte operand, string comment) : base(ECommand.SETIXH, operand, comment) { }
    }

    internal class SETIXL : ByteOperandCommand
    {
        public SETIXL(byte operand, string comment) : base(ECommand.SETIXL, operand, comment) { }
    }

    internal class LDIA : ByteOperandCommand
    {
        public LDIA(byte operand, string comment) : base(ECommand.LDIA, operand, comment) { }
    }

    internal class LDIB : ByteOperandCommand
    {
        public LDIB(byte operand, string comment) : base(ECommand.LDIB, operand, comment) { }
    }

    internal class STDI : ByteOperandCommand
    {
        public STDI(byte operand, string comment) : base(ECommand.STDI, operand, comment) { }
    }
}
