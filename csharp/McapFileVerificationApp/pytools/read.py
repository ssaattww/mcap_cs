import sys
from mcap.reader import make_reader
from mcap.records import Channel, Schema, Message

# MCAPファイルをバイナリ読み込みモードで開きます
# C#アプリケーションが生成した output.mcap を読み込みます（固定パス例）
with open("csharp/McapFileVerificationApp/output.mcap", "rb") as f:
    # MCAPリーダーを作成します
    reader = make_reader(f)

    # すべてのメッセージを反復処理します（ヘッダ表示は省略/互換）
    for schema, channel, message in reader.iter_messages():
        print(f"Channel: {channel.topic} (ID: {channel.id})")
        print(f"Schema: {schema.name} (ID: {schema.id})")
        try:
            payload = message.data.decode("utf-8")
        except Exception:
            payload = f"<{len(message.data)} bytes>"
        print(f"Message on channel {message.channel_id} at {message.log_time}: {payload}")
