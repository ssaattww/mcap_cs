import sys
from mcap.reader import make_reader
from mcap.records import Channel, Schema, Message

# MCAPファイルをバイナリ読み込みモードで開きます
# C#アプリケーションが生成したoutput.mcapを読み込みます
with open("output.mcap", "rb") as f:
    # MCAPリーダーを作成します
    reader = make_reader(f)
    
    # すべてのメッセージを反復処理します
    for schema, channel, message in reader.iter_messages():
        print(f"Channel: {channel.topic} (ID: {channel.id})")
        print(f"Schema: {schema.name} (ID: {schema.id})")
        print(f"Message on channel {message.channel_id} at {message.log_time}: {message.data.decode('utf-8')}")
