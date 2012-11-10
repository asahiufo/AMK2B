AMK2B
=====
Kinect の スケルトンデータを Blender へ送信し、Blender の bone へ適用することで、簡単なモーションキャプチャを実現するツールです。

実行環境
--------
* Microsoft Windows 7 以上
* [NET Framework 4 Client Profile](http://www.microsoft.com/ja-jp/net/netfx4/download.aspx "NET Framework 4 Client Profile") 以上
* [Kinect for Windows](http://www.microsoft.com/en-us/kinectforwindows/ "Kinect for Windows")
* [Kinect for Windows Runtime v1.5](http://www.microsoft.com/en-us/download/details.aspx?id=34810 "Kinect for Windows Runtime v1.5") 以上
* [Blender 2.64](http://www.blender.org/ "Blender") 以上（2.5以上なら大丈夫かも？）

インストール
------------
1. [Downloads](https://github.com/asahiufo/AMK2B/downloads "Downloads") からツールをダウンロードします。
1. ダウンロードした圧縮ファイルを任意のフォルダへ展開します。  
   （以降、展開したフォルダのことを "`AMK2B`" と記載します。）
1. 展開したフォルダ内の Blender のアドオンを Blender へインストールします。
   1. 「`AMK2B/amk2b`」フォルダを「`<Blenderのインストールディレクトリ>/<バージョン>/scripts/addons`」へコピーします。
   1. Blender を起動し、「`File ＞ User Preferences...`」をクリックします。
   1. 「`Addons ＞ Categories ＞ Development`」と順にクリックします。
   1. 「`Development: AMK2B - Kinect Data Receiver`」の右端のボックスをチェックします。
   1. この結果、「`3D View`」の左に「`AMK2B`」パネルが追加されていれば成功です。

アンインストール
----------------
1. Blender へ追加した「`amk2b`」フォルダを削除します。
1. ダウンロードした「`AMK2B`」フォルダを削除します。

使い方
------
1. 「`AMK2B/KinectDataSender/KinectDataSender.exe`」を起動します。  
   「`Kinect Data Sender`」画面が開きます。
1. 「`Kinect Data Sender`」画面の「`詳細設定`」で、Skeleton データを送信したい部位にチェックを入れます。
1. チェックを入れたチェックボックスに対応するテキストボックスへ、座標情報を適用する Blender のボーン名を入力します。  
   入力内容を保存する場合は、「`ファイル ＞ パラメータファイルを保存`」でファイル出力できます。
1. Blender を起動し、「`AMK2B/sample.blend`」を開いておきます。
1. 「`Kinect Data Sender`」画面の「`Kinect Start`」ボタンを押下します。  
   （Kinect が正常に作動した場合、左上にカメラ画像が表示されます。  
   表示されない場合、「`カメラ設定`」の「`カメラ画像描画`」がチェックされているか確認して下さい。）
1. 「`Kinect Data Sender`」画面の「`全体設定`」の「`自動設定`」ボタンを押下します。  
   カウントダウンが始まりますので、Kinect に向かって Blender 上のモデルと同じポーズをとります。  
   （"`前回設定時間`"が表示されれば成功です。）
   ここで設定されたジョイントの座標を基に相対座標で Blender の bone へ座標が適用されます。
1. Blender の「`AMK2B`」パネルの「`Receive Kinect Data`」ボタンを押下します。
1. Blender の「`AMK2B`」パネルの「`Apply Kinect Data`」ボタンを押下します。  
   これにより、Kinect の Skeleton 情報が Blender 上のモデルへ適用されます。  
   （体を動かすのと合わせてモデルも動けば成功です。）  
   なお、「Kinect Data Sender」の「ミラー」にチェックを入れると、  
   モデルに対して、鏡の前でポーズをとるように動かすことが出来るようになります。  
   他、「Kinect Data Sender」の設定や、モデルのボーンの設定などを見直し、  
   思うようなモーションキャプチャがとれるようになれるまで調整します。
1. Blender の「`AMK2B`」パネルの「`Recording`」ボタンを押下します。  
   カウントダウンが開始され、0 になるとモーションの録画が始まります。  
   録画は最初のフレームから最後のフレームまで行われ、停止します。
1. 出来上がったモデルとアニメーションデータを煮るなり焼くなり、Flash で使うなり。

Licensing
---------
Copyright &copy; 2012 asahiufo  
Licensed under the [GNU General Public License Version 3][GNU v3] 

Open Source Licensing
---------------------
* [Livet](http://ugaya40.net/livet "Livet")  
  Licensed under the [zlib/libpng][zlib/libpng]
* [Bespoke Open Sound Control Library](http://www.bespokesoftware.org/wordpress/?page_id=69 "Bespoke Open Sound Control Library")  
  Licensed under the [Microsoft Public License (MS-PL)][MS-PL]
* python-osc  
  Licensed under the [GNU General Public License][GNU]
