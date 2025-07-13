# ParseKeyValueMap

`ParseKeyValueMap` は、バイト配列から長さプレフィックスを持つキー/値のマップを `KeyValueMap` (`std::unordered_map<std::string, std::string>`) として解析するインライン関数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
inline Status ParseKeyValueMap(const std::byte* data, uint64_t maxSize, KeyValueMap* output) {
  uint32_t sizeInBytes = 0;
  if (auto status = ParseUint32(data, maxSize, &sizeInBytes); !status.ok()) {
    return status;
  }
  if (sizeInBytes > (maxSize - 4)) {
    const auto msg =
      StrCat("key-value map size ", sizeInBytes, " exceeds remaining bytes ", (maxSize - 4));
    return Status(StatusCode::InvalidRecord, msg);
  }

  sizeInBytes += 4; // Account for the byte size prefix in sizeInBytes

  output->clear();
  uint64_t pos = 4;
  while (pos < sizeInBytes) {
    std::string_view key;
    if (auto status = ParseStringView(data + pos, sizeInBytes - pos, &key); !status.ok()) {
      const auto msg = StrCat("cannot read key-value map key at pos ", pos, ": ", status.message);
      return Status{StatusCode::InvalidRecord, msg};
    }
    pos += 4 + key.size();
    std::string_view value;
    if (auto status = ParseStringView(data + pos, sizeInBytes - pos, &value); !status.ok()) {
      const auto msg = StrCat("cannot read key-value map value for key \"", key, "\" at pos ", pos,
                              ": ", status.message);
      return Status{StatusCode::InvalidRecord, msg};
    }
    pos += 4 + value.size();
    output->emplace(key, value);
  }
  return StatusCode::Success;
}
```

**概要:**

この関数は、まずマップ全体のサイズを示す4バイトのプレフィックスを読み取ります。その後、そのサイズ内でキーと値のペアを繰り返し解析します。各キーと値は、それぞれ4バイトの長さプレフィックスを持つ文字列として解析されます。解析されたキーと値のペアは `output` マップに追加されます。成功した場合は `StatusCode::Success` を返します。
