using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CProcessorAssembler.Commands
{
    internal abstract class Command
    {
        public Command(ECommand command, string comment)
        {
            CommandType = command;
            Comment = $"{command}\t{comment.Trim().TrimStart('-')}".Trim();
        }

        public ECommand CommandType { get; }
        public ushort MemoryAddress { get; set; }
        public string Comment { get; set; }

        public ushort GetByteLen()
        {
            return CommandType switch
            {
                ECommand.SETIX => 4,
                ECommand.JP or ECommand.JPC or ECommand.JPZ => 3,
                ECommand.SETIXH or ECommand.SETIXL or ECommand.LDIA or ECommand.LDIB or ECommand.STDI
                or ECommand.SLL or ECommand.SRL or ECommand.SLA or ECommand.SRA => 2,
                _ => 1,
            };
        }

        public virtual byte[] ToBytes(Dictionary<string, Command> labels, EEndianness endianness = EEndianness.Big)
        {
            return new byte[] { (byte)CommandType };

            //return OperandByteLen switch
            //{
            //    0 => new byte[] { (byte)CommandType },
            //    1 => new byte[] { (byte)CommandType, (byte)((byte)Operand & 0xff) },
            //    2 => endianness switch
            //    {
            //        EEndianness.Big => new byte[] {
            //                        (byte)CommandType,
            //                        (byte)((Operand & 0xff00) >> 8),
            //                        (byte)(Operand & 0xff),
            //                    },
            //        EEndianness.Little => new byte[] {
            //                        (byte)CommandType,
            //                        (byte)(Operand & 0xff),
            //                        (byte)((Operand & 0xff00) >> 8),
            //                    },
            //        _ => throw new NotSupportedException("Not supported endianness．"),
            //    },
            //    _ => throw new NotSupportedException("Not supported operand length."),
            //};
        }
    }

    //internal enum EOperator
    //{
    //    SETIXH = 0xd0,
    //    SETIXL = 0xd1,

    //    LDIA = 0xd8,
    //    LDIB = 0xd9,
    //    LDDA = 0xe0,
    //    LDDB = 0xe1,

    //    STDA = 0xf0,
    //    STDB = 0xf4,
    //    STDI = 0xf8,

    //    ADDA = 0x80,
    //    SUBA = 0x81,
    //    ANDA = 0x82,
    //    ORA = 0x83,
    //    NOTA = 0x84,
    //    INCA = 0x85,
    //    DECA = 0x86,
    //    ADDB = 0x90,
    //    SUBB = 0x91,
    //    ANDB = 0x92,
    //    ORB = 0x93,
    //    NOTB = 0x98,
    //    INCB = 0x99,
    //    DECB = 0x9a,
    //    CMP = 0xa1,

    //    NOP = 0x00,
    //    JP = 0x60,
    //    JPC = 0x40,
    //    JPZ = 0x50,
    //}

    internal enum ECommand
    {
        SETIX = 0x0f00,

        SETIXH = 0xd0,
        SETIXL = 0xd1,

        LDIA = 0xd8,
        LDIB = 0xd9,
        LDDA = 0xe0,
        LDDB = 0xe1,

        STDA = 0xf0,
        STDB = 0xf4,
        STDI = 0xf8,

        ADDA = 0x80,
        SUBA = 0x81,
        ANDA = 0x82,
        ORA = 0x83,
        NOTA = 0x84,
        INCA = 0x85,
        DECA = 0x86,
        ADDB = 0x90,
        SUBB = 0x91,
        ANDB = 0x92,
        ORB = 0x93,
        NOTB = 0x98,
        INCB = 0x99,
        DECB = 0x9a,
        CMP = 0xa1,

        SLL = 0x70,
        SRL = 0x71,
        SLA = 0x72,
        SRA = 0x73,

        NOP = 0x00,
        JP = 0x60,
        JPC = 0x40,
        JPZ = 0x50,
    }
}
