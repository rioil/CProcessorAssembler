using CProcessorAssembler.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CProcessorAssembler.Commands
{
    internal abstract class AddressOperandCommand : Command
    {
        public AddressOperandCommand(ECommand command, ushort address, string comment) : base(command, comment)
        {
            Address = address;
        }

        public AddressOperandCommand(ECommand command, string? labelOrAddress, string comment) : base(command, comment)
        {
            if (string.IsNullOrEmpty(labelOrAddress)) { throw new ArgumentException(); }

            if (labelOrAddress.StartsWith(":")) {
                Label = labelOrAddress[1..];
            }
            else {
                Address = (ushort)StringUtil.ParseNumericString(labelOrAddress);
            }
        }

        public ushort Address { get; private set; }
        public string? Label { get; }

        public override byte[] ToBytes(Dictionary<string, Command> labels, EEndianness endianness = EEndianness.Big)
        {
            if (Label is not null) {
                Address = labels[Label].MemoryAddress;
            }

            return endianness switch
            {
                EEndianness.Big => new byte[] {
                                    (byte)CommandType,
                                    (byte)((Address & 0xff00) >> 8),
                                    (byte)(Address & 0xff),
                                },
                EEndianness.Little => new byte[] {
                                    (byte)CommandType,
                                    (byte)(Address & 0xff),
                                    (byte)((Address & 0xff00) >> 8),
                                },
                _ => throw new NotSupportedException("Not supported endianness．"),
            };
        }
    }

    internal class SETIX : AddressOperandCommand
    {
        public SETIX(ushort address, string comment) : base(ECommand.SETIX, address, comment) { }
        public SETIX(string? labelOrAddress, string comment) : base(ECommand.SETIX, labelOrAddress, comment) { }

        public override byte[] ToBytes(Dictionary<string, Command> labels, EEndianness endianness = EEndianness.Big)
        {
            return new byte[] { (byte)ECommand.SETIXH, (byte)((Address & 0xff00) >> 8), (byte)ECommand.SETIXL, (byte)(Address & 0xff) };
        }
    }

    internal class JP : AddressOperandCommand
    {
        public JP(ushort address, string comment) : base(ECommand.JP, address, comment) { }
        public JP(string? labelOrAddress, string comment) : base(ECommand.JP, labelOrAddress, comment) { }
    }

    internal class JPC : AddressOperandCommand
    {
        public JPC(ushort address, string comment) : base(ECommand.JPC, address, comment) { }
        public JPC(string? labelOrAddress, string comment) : base(ECommand.JPC, labelOrAddress, comment) { }
    }

    internal class JPZ : AddressOperandCommand
    {
        public JPZ(ushort address, string comment) : base(ECommand.JPZ, address, comment) { }
        public JPZ(string? labelOrAddress, string comment) : base(ECommand.JPZ, labelOrAddress, comment) { }
    }
}
