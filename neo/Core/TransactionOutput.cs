﻿using Neo.IO;
using Neo.IO.Json;
using Neo.VM;
using Neo.Wallets;
using System;
using System.IO;

namespace Neo.Core
{
    /// <summary>
    /// 交易输出
	/// Transaction output
    /// </summary>
    public class TransactionOutput : IInteropInterface, ISerializable
    {
		/// <summary>
		/// 资产编号
		/// asset ID
		/// </summary>
		public UInt256 AssetId;
		/// <summary>
		/// 金额
		/// amount
		/// </summary>
		public Fixed8 Value;
        /// <summary>
        /// 收款地址
		/// Recipient Address
        /// </summary>
        public UInt160 ScriptHash;

        public int Size => AssetId.Size + Value.Size + ScriptHash.Size;

        void ISerializable.Deserialize(BinaryReader reader)
        {
            this.AssetId = reader.ReadSerializable<UInt256>();
            this.Value = reader.ReadSerializable<Fixed8>();
            if (Value <= Fixed8.Zero) throw new FormatException();
            this.ScriptHash = reader.ReadSerializable<UInt160>();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(Value);
            writer.Write(ScriptHash);
        }

		/// <summary>
		/// 将交易输出转变为json对象
		/// Turn the transaction output into a json object
		/// </summary>
		/// <param name="index">该交易输出在交易中的索引 The index of the transaction output in the transaction</param>
		/// <returns>返回json对象 returns json object</returns>
		public JObject ToJson(ushort index)
        {
            JObject json = new JObject();
            json["n"] = index;
            json["asset"] = AssetId.ToString();
            json["value"] = Value.ToString();
            json["address"] = Wallet.ToAddress(ScriptHash);
            return json;
        }
    }
}
