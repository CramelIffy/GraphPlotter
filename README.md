# GraphPlotter
燃焼実験によって得られたデータを解析、及び描画するプログラムです。

## 注意
**これは旧バージョンです！**
v2.x.x系は[こちら](https://github.com/CramelIffy/GraphPlotter2 "GraphPlotter2")

## How to use
1. 起動して、`Select File`を押して読み込むファイルを選択してください。
2. 読み込むファイルのデータの単位が、ミリボルトではなくニュートンであったり、ミリ秒ではなく秒であったりする場合は右上のチェックボックスを操作してください。
3. `Plot`ボタンを押します。
4. 描画されます。
5. 複数データを描画したいときは1~4までの操作をもう一度行ってください。
6. できたグラフを画像として保存したいときは`Save Figure`を押してください。

## 読み込むファイルについて
csvファイルを読み込んでください。ここで、ヘッダ行があった場合は自動で無視されます。

## 注意事項
1. 時間データは必ず**増加**し続ける必要があります。(増加し続けていない場合ソートします。)
2. ヘッダ行以外のデータの一部に数値データ以外が入っていた場合エラーとなります。(その行は読み飛ばされ、警告が表示されます。)
3. 燃焼時間の推定において、最大出力のN％のデータが連続したら燃焼開始判定を行う等、最大出力を基準として計算する為、非常に大きな外れ値等が存在する場合推定がうまくできません。そのため、うまく燃焼時間が推定できなかった場合は自動で原因となっているであろう箇所までスキップします。
4. トータルインパルスの計算に用いるデータはデノイズの行われていない元データを用います。ここで、デノイズに用いたアルゴリズムはサビツキーゴーレイフィルタです。幅は21で、4次関数による近似を用いたデノイズです。
5. 燃焼開始時刻と燃焼終了時刻が乱れる場合があります。

## 各チェックボックスの説明
### Read as Volutage
スラストデータを電圧値[mv]として読み込みます。このチェックボックスにチェックが入っているときは、GraphPlotter.exeと同一ディレクトリ内に置かれたCalibrationData.csvを参照して電圧値をニュートンへと変換します。この時用いる重力加速度は $9.796685 [m/s^2]$ です。CalibrationData.csvのフォーマットは以下のとおりです。
|  kg  |  mv  |
| ---- | ---- |
|  0.00  |  230.62  |
|  1.30  |  275.53  |
| 以下略 | 以下略 |

このように、一列目にkg、二列目にmv値が書かれたファイルを用います。
尚、チェックボックスにチェックが入っていないときはニュートンとしてファイルを読み込みます。
また、Releaseからダウンロードしたファイルの中にもともと同封されているCalibrationData.csvは仮のものです。正しい値を得る場合には自分でCalibrationData.csvを書き換えてください。

### Read as Millisecond
時間データをミリ秒として読み込みます。チェックボックスにチェックが入っていないときは秒として読み込みます。

### Plot Maximum Value
最大値をグラフに描画します。このとき、描画される最大値はデノイズされていない元データの最大値です。

### Plot Total Impulse
全力積を計算し、グラフの右上に描画します。尚、全力積は燃焼中のデータのみを用いて計算します。ここで、計算式は以下のようになっています。

$$
Impulse = \frac{\Sigma_{i = 0}^{N - 1} (F_i + F_{i + 1})(t_{i + 1} - t_i)}{2}
$$

ここで、 $N$ は燃焼開始から終了までのデータ数、 $F_i$ はスラスト、 $t_i$ は秒数です。

### Plot Burn time
燃焼時間をグラフ下部に描画します。なお、燃焼時間の推定方法は燃焼時間は燃焼開始時刻と燃焼終了時刻を推定することで算出しています。**なお、推定に用いるのはノイズ除去済みデータです。**
1. 燃焼開始時刻推定: データの一番初めから順に最大推力の`IgnitionDetectionThreshold`の値*100%の値が連続で500回連続で検出されたとき、検出され始めた地点を燃焼開始時刻とする。
2. 燃焼終了時刻推定: データの一番後ろから順に最大推力の`BurnoutDetectionThreshold`の値*100%の値が連続で1000回連続で検出されたとき、検出され始めた地点を燃焼終了時刻とする。

### Plot Average Thrust
平均推力を描画します。平均推力は燃焼開始から終了までの平均推力を計算しています。

### Set the IgnitionTime to 0[s]
次に描画するグラフデータの燃焼時間を0秒地点に揃えます。

### Align the Maximum Value
次に描画するグラフデータの最大値を記録した地点を1つ目のグラフの最大値を記録した地点と揃えます。

### Denoise
画面に描画するグラフのノイズ成分を除去します。ここでノイズ除去に用いるアルゴリズムはサビツキーゴーレイフィルタです。尚、このチェックボックスのチェックの有無に関わらずデータ処理に用いるデータがノイズ除去済みのものであるか、元データであるかは固定です。用いるデータは以下のようになっています。

| 計算項目 | 用いるデータ |
| --- | --- |
| 燃焼時間推定 | ノイズ除去済み |
| 最大値 | 元データ |
| 全力積 | 元データ |
| 平均推力 | 元データ |

### Denoised vs Raw
ノイズ除去済みのデータと元データを同時に描画します。元データのほうが半透明であり、またノイズ除去済みのデータに比べて線が若干太いです。

### Offset Removal
データのオフセットを検出して除去します。このチェックボックスにチェックが入っている場合、オフセット除去済みのデータを元データとして用います。即ちこのチェックボックスのチェックの有無によって計算結果が変動します。
オフセットの検出方法は、以下のようになります。

1. スラストデータを小さい順に並び替える。
2. スラストデータの個数を取得する。(以下において、Nとする。)
3. N * 0.03 ~ N * 0.06の範囲を平均する。

上記の手順に得られた値をオフセットとします。その後、全スラストデータに対しこのオフセット値を減算することでオフセット除去を行います。

### Show PeakProtection Intensity
ピーク保護において、ピーク保護強度をグラフに描画します。Denoiseを行うとピークが鈍る傾向にあります。そのため、ピーク保護を設定することで、ピークを保護する事ができます。ピーク保護の強度はこのチェックボックスの下にあるスライダを調節することで変更できます。デフォルトでは0になっており、0ではピーク保護をしません。

ピーク保護は以下のような手順で行われます。

1. まずピーク保護強度を計算します
    1. スラストデータにハイパスフィルタを通します。
    1. 絶対値を取得します。(このデータを $abs_i$ とします。)
    1. 最大値を取得します。(以下 $max$ とします。)
    1. スライダで設定した値でMaxを割ります。(以下 $IntensityValue$ とします。)
    1. ピーク保護強度( $p_i$ )を計算します。計算式は次のようになります。
    $$p_i = \frac{1}{1 + e^{(\frac{-20(abs_i - IntensityValue)}{max})}}$$ 
    1.  $p_i$ が0.5より大きいとき1に、0.5以下のとき $(\frac{p_i}{2})^4$ とします。
    1. 上記の手順によって得られた $p_i$ をピーク保護強度とします。
1. 元データ( $F_i$ )とノイズ除去済みのデータ( $D_i$ )を以下のように混ぜます。
    1.  $F_i \times p_i + D_i \times (1 - p_i)$
    1. 上記の式によって得られた値がピーク保護済みのデータです。
