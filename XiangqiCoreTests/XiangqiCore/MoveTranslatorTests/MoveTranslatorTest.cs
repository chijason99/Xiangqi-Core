using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Move;

namespace xiangqi_core_test.XiangqiCore.MoveTranslatorTests;

public static class MoveTranslatorTest
{
	public static IEnumerable<object[]> MoveTranslatorEnglishToUcciTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 B3+5 c2=4\r\n2 N8+9 n2+3\r\n3 R9=8 r1=2\r\n4 P3+1 n8+7\r\n5 N2+3 p3+1\r\n6 A4+5 b7+5\r\n7 P9+1 a6+5\r\n8 R1=4 r2+3\r\n9 C8=6 r2+6\r\n10 N9-8 c8=9\r\n11 C2+4 r9=8\r\n12 R4=2 r8=6\r\n13 N8+9 r6+4\r\n14 C2+3 n7-8\r\n15 R2+9 a5-6\r\n16 R2-3 p7+1\r\n17 R2=1 p7+1\r\n18 B5+3 c9=7\r\n19 R1=3 c7-2\r\n20 B7+5 c4+1\r\n21 R3+2 a6+5\r\n22 C6=7 n3+4\r\n23 P1+1 r6+2\r\n24 C7+3 r6=7\r\n25 N3-4 r7=5\r\n26 C7+1 n4+6\r\n27 R3=4 n6+4\r\n28 C7=8 n4-5\r\n29 C8=5 n5+6\r\n30 C5=3 c7+5\r\n31 R4-4 c7=1\r\n32 N4+3 r5+1\r\n33 R4-1 r5=1\r\n34 C3=5 r1=7\r\n35 K5=4 r7+2\r\n36 K4+1 c4+5",
					MoveNotationType.English,
					"g0e2 b7d7 b0a2 b9c7 a0b0 a9b9 g3g4 h9g7 h0g2 c6c5 f0e1 g9e7 a3a4 f9e8 i0f0 b9b6 b2d2 b6b0 a2b0 h7i7 h2h6 i9h9 f0h0 h9f9 b0a2 f9f5 h6h9 g7h9 h0h9 e8f9 h9h6 g6g5 h6i6 g5g4 e2g4 i7g7 i6g6 g7g9 c0e2 d7d6 g6g8 f9e8 d2c2 c7d5 i3i4 f5f3 c2c5 f3g3 g2f0 g3e3 c5c6 d5f4 g8f8 f4d3 c6b6 d3e5 b6e6 e5f3 e6g6 g9g4 f8f4 g4a4 f0g2 e3e2 f4f3 e2a2 g6e6 a2g2 e0f0 g2g0 f0f1 d6d1",
					MoveNotationType.UCCI)
			};			
			
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 C2=5 n8+7\r\n2 N2+3 r9=8\r\n3 R1=2 n2+3\r\n4 P7+1 p7+1\r\n5 R2+6 n7+6\r\n6 N8+7 b3+5\r\n7 P5+1 p7+1\r\n8 R2=4 n6+7\r\n9 R4=3 n7-5\r\n10 N7+5 p7+1\r\n11 C5+2 p7+1\r\n12 R3=2 r1+1\r\n13 C5=2 b5-3\r\n14 C8=5 p7=6\r\n15 C5-1 r8+1\r\n16 R9=8 c8+3\r\n17 R2+2 r1=8\r\n18 R8+7 b7+5\r\n19 R8=7 r8=7\r\n20 C5+5 a6+5\r\n21 B7+5 r7+5\r\n22 N5+4 r7-2\r\n23 N4-6 c8=5\r\n24 A6+5 r7=4\r\n25 N6-7 r4+3\r\n26 R7+2 k5=6\r\n27 R7-3 p6+1\r\n28 C5=4 r4=3\r\n29 K5=6 r3=5\r\n30 C4=9 r5-1\r\n31 R7=4 k6=5\r\n32 R4-5 r5=4\r\n33 A5+6 r4=1\r\n34 R4+3 r1-3\r\n35 R4=5 r1+3\r\n36 P1+1 r1-2\r\n37 R5+3 p9+1",
					MoveNotationType.English,
					"h2e2 h9g7 h0g2 i9h9 i0h0 b9c7 c3c4 g6g5 h0h6 g7f5 b0c2 c9e7 e3e4 g5g4 h6f6 f5g3 f6g6 g3e4 c2e3 g4g3 e2e4 g3g2 g6h6 a9a8 e4h4 e7c9 b2e2 g2f2 e2e1 h9h8 a0b0 h7h4 h6h8 a8h8 b0b7 g9e7 b7c7 h8g8 e1e6 f9e8 c0e2 g8g3 e3f5 g3g5 f5d4 h4e4 d0e1 g5d5 d4c2 d5d2 c7c9 e9f9 c9c6 f2f1 e6f6 d2c2 e0d0 c2e2 f6a6 e2e3 c6f6 f9e9 f6f1 e3d3 e1d2 d3a3 f1f4 a3a6 f4e4 a6a3 i3i4 a3a5 e4e7 i6i5",
					MoveNotationType.UCCI)
			};
		}
	}

	public static IEnumerable<object[]> MoveTranslatorEnglishToTraditionalChineseTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 C2=5 n8+7\r\n2 N2+3 r9=8\r\n3 R1=2 n2+3\r\n4 P7+1 p7+1\r\n5 R2+6 n7+6\r\n6 N8+7 b3+5\r\n7 P5+1 p7+1\r\n8 R2=4 n6+7\r\n9 R4=3 n7-5\r\n10 N7+5 p7+1\r\n11 C5+2 p7+1\r\n12 R3=2 r1+1\r\n13 C5=2 b5-3\r\n14 C8=5 p7=6\r\n15 C5-1 r8+1\r\n16 R9=8 c8+3\r\n17 R2+2 r1=8\r\n18 R8+7 b7+5\r\n19 R8=7 r8=7\r\n20 C5+5 a6+5\r\n21 B7+5 r7+5\r\n22 N5+4 r7-2\r\n23 N4-6 c8=5\r\n24 A6+5 r7=4\r\n25 N6-7 r4+3\r\n26 R7+2 k5=6\r\n27 R7-3 p6+1\r\n28 C5=4 r4=3\r\n29 K5=6 r3=5\r\n30 C4=9 r5-1\r\n31 R7=4 k6=5\r\n32 R4-5 r5=4\r\n33 A5+6 r4=1\r\n34 R4+3 r1-3\r\n35 R4=5 r1+3\r\n36 P1+1 r1-2\r\n37 R5+3 p9+1",
					MoveNotationType.English,
					"炮二平五 馬８進７ 馬二進三 車９平８ 車一平二 馬２進３ 兵七進一 卒７進１ 車二進六 馬７進６ 馬八進七 象３進５ 兵五進一 卒７進１ 車二平四 馬６進７ 車四平三 馬７退５ 馬七進五 卒７進１ 炮五進二 卒７進１ 車三平二 車１進１ 炮五平二 象５退３ 炮八平五 卒７平６ 炮五退一 車８進１ 車九平八 炮８進３ 車二進二 車１平８ 車八進七 象７進５ 車八平七 車８平７ 炮五進五 士６進５ 相七進五 車７進５ 馬五進四 車７退２ 馬四退六 炮８平５ 仕六進五 車７平４ 馬六退七 車４進３ 車七進二 將５平６ 車七退三 卒６進１ 炮五平四 車４平３ 帥五平六 車３平５ 炮四平九 車５退１ 車七平四 將６平５ 車四退五 車５平４ 仕五進六 車４平１ 車四進三 車１退３ 車四平五 車１進３ 兵一進一 車１退２ 車五進三 卒９進１",
					MoveNotationType.TraditionalChinese)
			};

			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 C2=6 n8+7\r\n2 N2+3 p7+1\r\n3 R1=2 r9=8\r\n4 R2+4 c8=9\r\n5 R2=6 n2+3\r\n6 P7+1 c2=1\r\n7 N8+7 r1=2\r\n8 P7+1 p3+1\r\n9 N7+8 c1=2\r\n10 N8+9 c2=1\r\n11 N9+7 c9=3\r\n12 R9=8 r2+6\r\n13 C6+7 a6+5\r\n14 C6-1 c1=2\r\n15 C6=8 c2+5\r\n16 C8-6 r8+6\r\n17 B7+5 r8=7\r\n18 N3-2 r7=5\r\n19 A6+5 b7+5\r\n20 P1+1 p5+1\r\n21 N2+1 r5-1\r\n22 R6-2 c3=2\r\n23 K5=6 r5=2",
					MoveNotationType.English,
					"炮二平六 馬８進７ 馬二進三 卒７進１ 車一平二 車９平８ 車二進四 炮８平９ 車二平六 馬２進３ 兵七進一 炮２平１ 馬八進七 車１平２ 兵七進一 卒３進１ 馬七進八 炮１平２ 馬八進九 炮２平１ 馬九進七 炮９平３ 車九平八 車２進６ 炮六進七 士６進５ 炮六退一 炮１平２ 炮六平八 炮２進５ 炮八退六 車８進６ 相七進五 車８平７ 馬三退二 車７平５ 仕六進五 象７進５ 兵一進一 卒５進１ 馬二進一 車５退１ 車六退二 炮３平２ 帥五平六 車５平２",
					MoveNotationType.TraditionalChinese)
			};
		}
	}	
	
	public static IEnumerable<object[]> MoveTranslatorEnglishToSimplifiedChineseTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 C2=5 n8+7\r\n2 N2+3 r9=8\r\n3 R1=2 n2+3\r\n4 P7+1 p7+1\r\n5 R2+6 n7+6\r\n6 N8+7 b3+5\r\n7 P5+1 p7+1\r\n8 R2=4 n6+7\r\n9 R4=3 n7-5\r\n10 N7+5 p7+1\r\n11 C5+2 p7+1\r\n12 R3=2 r1+1\r\n13 C5=2 b5-3\r\n14 C8=5 p7=6\r\n15 C5-1 r8+1\r\n16 R9=8 c8+3\r\n17 R2+2 r1=8\r\n18 R8+7 b7+5\r\n19 R8=7 r8=7\r\n20 C5+5 a6+5\r\n21 B7+5 r7+5\r\n22 N5+4 r7-2\r\n23 N4-6 c8=5\r\n24 A6+5 r7=4\r\n25 N6-7 r4+3\r\n26 R7+2 k5=6\r\n27 R7-3 p6+1\r\n28 C5=4 r4=3\r\n29 K5=6 r3=5\r\n30 C4=9 r5-1\r\n31 R7=4 k6=5\r\n32 R4-5 r5=4\r\n33 A5+6 r4=1\r\n34 R4+3 r1-3\r\n35 R4=5 r1+3\r\n36 P1+1 r1-2\r\n37 R5+3 p9+1",
					MoveNotationType.English,
					"炮二平五 马８进７ 马二进三 车９平８ 车一平二 马２进３ 兵七进一 卒７进１ 车二进六 马７进６ 马八进七 象３进５ 兵五进一 卒７进１ 车二平四 马６进７ 车四平三 马７退５ 马七进五 卒７进１ 炮五进二 卒７进１ 车三平二 车１进１ 炮五平二 象５退３ 炮八平五 卒７平６ 炮五退一 车８进１ 车九平八 炮８进３ 车二进二 车１平８ 车八进七 象７进５ 车八平七 车８平７ 炮五进五 士６进５ 相七进五 车７进５ 马五进四 车７退２ 马四退六 炮８平５ 仕六进五 车７平４ 马六退七 车４进３ 车七进二 将５平６ 车七退三 卒６进１ 炮五平四 车４平３ 帅五平六 车３平５ 炮四平九 车５退１ 车七平四 将６平５ 车四退五 车５平４ 仕五进六 车４平１ 车四进三 车１退３ 车四平五 车１进３ 兵一进一 车１退２ 车五进三 卒９进１",
					MoveNotationType.SimplifiedChinese)
			};			
			
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 C2=6 n8+7\r\n2 N2+3 p7+1\r\n3 R1=2 r9=8\r\n4 R2+4 c8=9\r\n5 R2=6 n2+3\r\n6 P7+1 c2=1\r\n7 N8+7 r1=2\r\n8 P7+1 p3+1\r\n9 N7+8 c1=2\r\n10 N8+9 c2=1\r\n11 N9+7 c9=3\r\n12 R9=8 r2+6\r\n13 C6+7 a6+5\r\n14 C6-1 c1=2\r\n15 C6=8 c2+5\r\n16 C8-6 r8+6\r\n17 B7+5 r8=7\r\n18 N3-2 r7=5\r\n19 A6+5 b7+5\r\n20 P1+1 p5+1\r\n21 N2+1 r5-1\r\n22 R6-2 c3=2\r\n23 K5=6 r5=2",
					MoveNotationType.English,
					"炮二平六 马８进７ 马二进三 卒７进１ 车一平二 车９平８ 车二进四 炮８平９ 车二平六 马２进３ 兵七进一 炮２平１ 马八进七 车１平２ 兵七进一 卒３进１ 马七进八 炮１平２ 马八进九 炮２平１ 马九进七 炮９平３ 车九平八 车２进６ 炮六进七 士６进５ 炮六退一 炮１平２ 炮六平八 炮２进５ 炮八退六 车８进６ 相七进五 车８平７ 马三退二 车７平５ 仕六进五 象７进５ 兵一进一 卒５进１ 马二进一 车５退１ 车六退二 炮３平２ 帅五平六 车５平２",
					MoveNotationType.SimplifiedChinese)
			};
		}
	}
	
	public static IEnumerable<object[]> MoveTranslatorTraditionalChineseToSimplifiedChineseTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平五 馬8進7\r\n2 馬二進三 車9平8\r\n3 車一平二 馬2進3\r\n4 兵七進一 卒7進1\r\n5 車二進六 馬7進6\r\n6 馬八進七 象3進5\r\n7 兵五進一 卒7進1\r\n8 車二平四 馬6進7\r\n9 車四平三 馬7退5\r\n10 馬七進五 卒7進1\r\n11 炮五進二 卒7進1\r\n12 車三平二 車1進1\r\n13 炮五平二 象5退3\r\n14 炮八平五 卒7平6\r\n15 炮五退一 車8進1\r\n16 車九平八 炮8進3\r\n17 車二進二 車1平8\r\n18 車八進七 象7進5\r\n19 車八平七 車8平7\r\n20 炮五進五 士6進5\r\n21 相七進五 車7進5\r\n22 馬五進四 車7退2\r\n23 馬四退六 炮8平5\r\n24 仕六進五 車7平4\r\n25 馬六退七 車4進3\r\n26 車七進二 將5平6\r\n27 車七退三 卒6進1\r\n28 炮五平四 車4平3\r\n29 帥五平六 車3平5\r\n30 炮四平九 車5退1\r\n31 車七平四 將6平5\r\n32 車四退五 車5平4\r\n33 仕五進六 車4平1\r\n34 車四進三 車1退3\r\n35 車四平五 車1進3\r\n36 兵一進一 車1退2\r\n37 車五進三 卒9進1",
					MoveNotationType.TraditionalChinese,
					"炮二平五 马８进７ 马二进三 车９平８ 车一平二 马２进３ 兵七进一 卒７进１ 车二进六 马７进６ 马八进七 象３进５ 兵五进一 卒７进１ 车二平四 马６进７ 车四平三 马７退５ 马七进五 卒７进１ 炮五进二 卒７进１ 车三平二 车１进１ 炮五平二 象５退３ 炮八平五 卒７平６ 炮五退一 车８进１ 车九平八 炮８进３ 车二进二 车１平８ 车八进七 象７进５ 车八平七 车８平７ 炮五进五 士６进５ 相七进五 车７进５ 马五进四 车７退２ 马四退六 炮８平５ 仕六进五 车７平４ 马六退七 车４进３ 车七进二 将５平６ 车七退三 卒６进１ 炮五平四 车４平３ 帅五平六 车３平５ 炮四平九 车５退１ 车七平四 将６平５ 车四退五 车５平４ 仕五进六 车４平１ 车四进三 车１退３ 车四平五 车１进３ 兵一进一 车１退２ 车五进三 卒９进１",
					MoveNotationType.SimplifiedChinese)
			};			
			
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平六 馬8進7\r\n2 馬二進三 卒7進1\r\n3 車一平二 車9平8\r\n4 車二進四 炮8平9\r\n5 車二平六 馬2進3\r\n6 兵七進一 炮2平1\r\n7 馬八進七 車1平2\r\n8 兵七進一 卒3進1\r\n9 馬七進八 炮1平2\r\n10 馬八進九 炮2平1\r\n11 馬九進七 炮9平3\r\n12 車九平八 車2進6\r\n13 炮六進七 士6進5\r\n14 炮六退一 炮1平2\r\n15 炮六平八 炮2進5\r\n16 炮八退六 車8進6\r\n17 相七進五 車8平7\r\n18 馬三退二 車7平5\r\n19 仕六進五 象7進5\r\n20 兵一進一 卒5進1\r\n21 馬二進一 車5退1\r\n22 車六退二 炮3平2\r\n23 帥五平六 車5平2",
					MoveNotationType.TraditionalChinese,
					"炮二平六 马８进７ 马二进三 卒７进１ 车一平二 车９平８ 车二进四 炮８平９ 车二平六 马２进３ 兵七进一 炮２平１ 马八进七 车１平２ 兵七进一 卒３进１ 马七进八 炮１平２ 马八进九 炮２平１ 马九进七 炮９平３ 车九平八 车２进６ 炮六进七 士６进５ 炮六退一 炮１平２ 炮六平八 炮２进５ 炮八退六 车８进６ 相七进五 车８平７ 马三退二 车７平５ 仕六进五 象７进５ 兵一进一 卒５进１ 马二进一 车５退１ 车六退二 炮３平２ 帅五平六 车５平２",
					MoveNotationType.SimplifiedChinese)
			};
		}
	}

	public static IEnumerable<object[]> MoveTranslatorTraditionalChineseToEnglishTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平五 馬8進7\r\n2 馬二進三 車9平8\r\n3 車一平二 馬2進3\r\n4 兵七進一 卒7進1\r\n5 車二進六 馬7進6\r\n6 馬八進七 象3進5\r\n7 兵五進一 卒7進1\r\n8 車二平四 馬6進7\r\n9 車四平三 馬7退5\r\n10 馬七進五 卒7進1\r\n11 炮五進二 卒7進1\r\n12 車三平二 車1進1\r\n13 炮五平二 象5退3\r\n14 炮八平五 卒7平6\r\n15 炮五退一 車8進1\r\n16 車九平八 炮8進3\r\n17 車二進二 車1平8\r\n18 車八進七 象7進5\r\n19 車八平七 車8平7\r\n20 炮五進五 士6進5\r\n21 相七進五 車7進5\r\n22 馬五進四 車7退2\r\n23 馬四退六 炮8平5\r\n24 仕六進五 車7平4\r\n25 馬六退七 車4進3\r\n26 車七進二 將5平6\r\n27 車七退三 卒6進1\r\n28 炮五平四 車4平3\r\n29 帥五平六 車3平5\r\n30 炮四平九 車5退1\r\n31 車七平四 將6平5\r\n32 車四退五 車5平4\r\n33 仕五進六 車4平1\r\n34 車四進三 車1退3\r\n35 車四平五 車1進3\r\n36 兵一進一 車1退2\r\n37 車五進三 卒9進1",
					MoveNotationType.TraditionalChinese,
					"C2=5 h8+7 H2+3 r9=8 R1=2 h2+3 P7+1 p7+1 R2+6 h7+6 H8+7 e3+5 P5+1 p7+1 R2=4 h6+7 R4=3 h7-5 H7+5 p7+1 C5+2 p7+1 R3=2 r1+1 C5=2 e5-3 C8=5 p7=6 C5-1 r8+1 R9=8 c8+3 R2+2 r1=8 R8+7 e7+5 R8=7 r8=7 C5+5 a6+5 E7+5 r7+5 H5+4 r7-2 H4-6 c8=5 A6+5 r7=4 H6-7 r4+3 R7+2 k5=6 R7-3 p6+1 C5=4 r4=3 K5=6 r3=5 C4=9 r5-1 R7=4 k6=5 R4-5 r5=4 A5+6 r4=1 R4+3 r1-3 R4=5 r1+3 P1+1 r1-2 R5+3 p9+1",
					MoveNotationType.English)
			};

			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平六 馬8進7\r\n2 馬二進三 卒7進1\r\n3 車一平二 車9平8\r\n4 車二進四 炮8平9\r\n5 車二平六 馬2進3\r\n6 兵七進一 炮2平1\r\n7 馬八進七 車1平2\r\n8 兵七進一 卒3進1\r\n9 馬七進八 炮1平2\r\n10 馬八進九 炮2平1\r\n11 馬九進七 炮9平3\r\n12 車九平八 車2進6\r\n13 炮六進七 士6進5\r\n14 炮六退一 炮1平2\r\n15 炮六平八 炮2進5\r\n16 炮八退六 車8進6\r\n17 相七進五 車8平7\r\n18 馬三退二 車7平5\r\n19 仕六進五 象7進5\r\n20 兵一進一 卒5進1\r\n21 馬二進一 車5退1\r\n22 車六退二 炮3平2\r\n23 帥五平六 車5平2",
					MoveNotationType.TraditionalChinese,
					"C2=6 h8+7 H2+3 p7+1 R1=2 r9=8 R2+4 c8=9 R2=6 h2+3 P7+1 c2=1 H8+7 r1=2 P7+1 p3+1 H7+8 c1=2 H8+9 c2=1 H9+7 c9=3 R9=8 r2+6 C6+7 a6+5 C6-1 c1=2 C6=8 c2+5 C8-6 r8+6 E7+5 r8=7 H3-2 r7=5 A6+5 e7+5 P1+1 p5+1 H2+1 r5-1 R6-2 c3=2 K5=6 r5=2",
					MoveNotationType.English)
			};
		}
	}

	public static IEnumerable<object[]> MoveTranslatorTraditionalChineseToUcciTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平五 馬8進7\r\n2 馬二進三 車9平8\r\n3 車一平二 馬2進3\r\n4 兵七進一 卒7進1\r\n5 車二進六 馬7進6\r\n6 馬八進七 象3進5\r\n7 兵五進一 卒7進1\r\n8 車二平四 馬6進7\r\n9 車四平三 馬7退5\r\n10 馬七進五 卒7進1\r\n11 炮五進二 卒7進1\r\n12 車三平二 車1進1\r\n13 炮五平二 象5退3\r\n14 炮八平五 卒7平6\r\n15 炮五退一 車8進1\r\n16 車九平八 炮8進3\r\n17 車二進二 車1平8\r\n18 車八進七 象7進5\r\n19 車八平七 車8平7\r\n20 炮五進五 士6進5\r\n21 相七進五 車7進5\r\n22 馬五進四 車7退2\r\n23 馬四退六 炮8平5\r\n24 仕六進五 車7平4\r\n25 馬六退七 車4進3\r\n26 車七進二 將5平6\r\n27 車七退三 卒6進1\r\n28 炮五平四 車4平3\r\n29 帥五平六 車3平5\r\n30 炮四平九 車5退1\r\n31 車七平四 將6平5\r\n32 車四退五 車5平4\r\n33 仕五進六 車4平1\r\n34 車四進三 車1退3\r\n35 車四平五 車1進3\r\n36 兵一進一 車1退2\r\n37 車五進三 卒9進1",
					MoveNotationType.TraditionalChinese,
					"h2e2 h9g7 h0g2 i9h9 i0h0 b9c7 c3c4 g6g5 h0h6 g7f5 b0c2 c9e7 e3e4 g5g4 h6f6 f5g3 f6g6 g3e4 c2e3 g4g3 e2e4 g3g2 g6h6 a9a8 e4h4 e7c9 b2e2 g2f2 e2e1 h9h8 a0b0 h7h4 h6h8 a8h8 b0b7 g9e7 b7c7 h8g8 e1e6 f9e8 c0e2 g8g3 e3f5 g3g5 f5d4 h4e4 d0e1 g5d5 d4c2 d5d2 c7c9 e9f9 c9c6 f2f1 e6f6 d2c2 e0d0 c2e2 f6a6 e2e3 c6f6 f9e9 f6f1 e3d3 e1d2 d3a3 f1f4 a3a6 f4e4 a6a3 i3i4 a3a5 e4e7 i6i5",
					MoveNotationType.UCCI)
			};

			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平六 馬8進7\r\n2 馬二進三 卒7進1\r\n3 車一平二 車9平8\r\n4 車二進四 炮8平9\r\n5 車二平六 馬2進3\r\n6 兵七進一 炮2平1\r\n7 馬八進七 車1平2\r\n8 兵七進一 卒3進1\r\n9 馬七進八 炮1平2\r\n10 馬八進九 炮2平1\r\n11 馬九進七 炮9平3\r\n12 車九平八 車2進6\r\n13 炮六進七 士6進5\r\n14 炮六退一 炮1平2\r\n15 炮六平八 炮2進5\r\n16 炮八退六 車8進6\r\n17 相七進五 車8平7\r\n18 馬三退二 車7平5\r\n19 仕六進五 象7進5\r\n20 兵一進一 卒5進1\r\n21 馬二進一 車5退1\r\n22 車六退二 炮3平2\r\n23 帥五平六 車5平2",
					MoveNotationType.TraditionalChinese,
					"h2d2 h9g7 h0g2 g6g5 i0h0 i9h9 h0h4 h7i7 h4d4 b9c7 c3c4 b7a7 b0c2 a9b9 c4c5 c6c5 c2b4 a7b7 b4a6 b7a7 a6c7 i7c7 a0b0 b9b3 d2d9 f9e8 d9d8 a7b7 d8b8 b7b2 b8b2 h9h3 c0e2 h3g3 g2h0 g3e3 d0e1 g9e7 i3i4 e6e5 h0i2 e3e4 d4d2 c7b7 e0d0 e4b4",
					MoveNotationType.UCCI)
			};
		}
	}

	public static IEnumerable<object[]> MoveTranslatorSimplifiedChineseToTraditionalChineseTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平五 马8进7\r\n2 马二进三 车9平8\r\n3 车一平二 马2进3\r\n4 兵七进一 卒7进1\r\n5 车二进六 马7进6\r\n6 马八进七 象3进5\r\n7 兵五进一 卒7进1\r\n8 车二平四 马6进7\r\n9 车四平三 马7退5\r\n10 马七进五 卒7进1\r\n11 炮五进二 卒7进1\r\n12 车三平二 车1进1\r\n13 炮五平二 象5退3\r\n14 炮八平五 卒7平6\r\n15 炮五退一 车8进1\r\n16 车九平八 炮8进3\r\n17 车二进二 车1平8\r\n18 车八进七 象7进5\r\n19 车八平七 车8平7\r\n20 炮五进五 士6进5\r\n21 相七进五 车7进5\r\n22 马五进四 车7退2\r\n23 马四退六 炮8平5\r\n24 仕六进五 车7平4\r\n25 马六退七 车4进3\r\n26 车七进二 将5平6\r\n27 车七退三 卒6进1\r\n28 炮五平四 车4平3\r\n29 帅五平六 车3平5\r\n30 炮四平九 车5退1\r\n31 车七平四 将6平5\r\n32 车四退五 车5平4\r\n33 仕五进六 车4平1\r\n34 车四进三 车1退3\r\n35 车四平五 车1进3\r\n36 兵一进一 车1退2\r\n37 车五进三 卒9进1",
					MoveNotationType.SimplifiedChinese,
					"炮二平五 馬８進７ 馬二進三 車９平８ 車一平二 馬２進３ 兵七進一 卒７進１ 車二進六 馬７進６ 馬八進七 象３進５ 兵五進一 卒７進１ 車二平四 馬６進７ 車四平三 馬７退５ 馬七進五 卒７進１ 炮五進二 卒７進１ 車三平二 車１進１ 炮五平二 象５退３ 炮八平五 卒７平６ 炮五退一 車８進１ 車九平八 炮８進３ 車二進二 車１平８ 車八進七 象７進５ 車八平七 車８平７ 炮五進五 士６進５ 相七進五 車７進５ 馬五進四 車７退２ 馬四退六 炮８平５ 仕六進五 車７平４ 馬六退七 車４進３ 車七進二 將５平６ 車七退三 卒６進１ 炮五平四 車４平３ 帥五平六 車３平５ 炮四平九 車５退１ 車七平四 將６平５ 車四退五 車５平４ 仕五進六 車４平１ 車四進三 車１退３ 車四平五 車１進３ 兵一進一 車１退２ 車五進三 卒９進１",
					MoveNotationType.TraditionalChinese)
			};

			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平六 马8进7\r\n2 马二进三 卒7进1\r\n3 车一平二 车9平8\r\n4 车二进四 炮8平9\r\n5 车二平六 马2进3\r\n6 兵七进一 炮2平1\r\n7 马八进七 车1平2\r\n8 兵七进一 卒3进1\r\n9 马七进八 炮1平2\r\n10 马八进九 炮2平1\r\n11 马九进七 炮9平3\r\n12 车九平八 车2进6\r\n13 炮六进七 士6进5\r\n14 炮六退一 炮1平2\r\n15 炮六平八 炮2进5\r\n16 炮八退六 车8进6\r\n17 相七进五 车8平7\r\n18 马三退二 车7平5\r\n19 仕六进五 象7进5\r\n20 兵一进一 卒5进1\r\n21 马二进一 车5退1\r\n22 车六退二 炮3平2\r\n23 帅五平六 车5平2",
					MoveNotationType.SimplifiedChinese,
					"炮二平六 馬８進７ 馬二進三 卒７進１ 車一平二 車９平８ 車二進四 炮８平９ 車二平六 馬２進３ 兵七進一 炮２平１ 馬八進七 車１平２ 兵七進一 卒３進１ 馬七進八 炮１平２ 馬八進九 炮２平１ 馬九進七 炮９平３ 車九平八 車２進６ 炮六進七 士６進５ 炮六退一 炮１平２ 炮六平八 炮２進５ 炮八退六 車８進６ 相七進五 車８平７ 馬三退二 車７平５ 仕六進五 象７進５ 兵一進一 卒５進１ 馬二進一 車５退１ 車六退二 炮３平２ 帥五平六 車５平２",
					MoveNotationType.TraditionalChinese)
			};
		}
	}

	public static IEnumerable<object[]> MoveTranslatorSimplifiedChineseToEnglishTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平五 马8进7\r\n2 马二进三 车9平8\r\n3 车一平二 马2进3\r\n4 兵七进一 卒7进1\r\n5 车二进六 马7进6\r\n6 马八进七 象3进5\r\n7 兵五进一 卒7进1\r\n8 车二平四 马6进7\r\n9 车四平三 马7退5\r\n10 马七进五 卒7进1\r\n11 炮五进二 卒7进1\r\n12 车三平二 车1进1\r\n13 炮五平二 象5退3\r\n14 炮八平五 卒7平6\r\n15 炮五退一 车8进1\r\n16 车九平八 炮8进3\r\n17 车二进二 车1平8\r\n18 车八进七 象7进5\r\n19 车八平七 车8平7\r\n20 炮五进五 士6进5\r\n21 相七进五 车7进5\r\n22 马五进四 车7退2\r\n23 马四退六 炮8平5\r\n24 仕六进五 车7平4\r\n25 马六退七 车4进3\r\n26 车七进二 将5平6\r\n27 车七退三 卒6进1\r\n28 炮五平四 车4平3\r\n29 帅五平六 车3平5\r\n30 炮四平九 车5退1\r\n31 车七平四 将6平5\r\n32 车四退五 车5平4\r\n33 仕五进六 车4平1\r\n34 车四进三 车1退3\r\n35 车四平五 车1进3\r\n36 兵一进一 车1退2\r\n37 车五进三 卒9进1",
					MoveNotationType.SimplifiedChinese,
					"C2=5 h8+7 H2+3 r9=8 R1=2 h2+3 P7+1 p7+1 R2+6 h7+6 H8+7 e3+5 P5+1 p7+1 R2=4 h6+7 R4=3 h7-5 H7+5 p7+1 C5+2 p7+1 R3=2 r1+1 C5=2 e5-3 C8=5 p7=6 C5-1 r8+1 R9=8 c8+3 R2+2 r1=8 R8+7 e7+5 R8=7 r8=7 C5+5 a6+5 E7+5 r7+5 H5+4 r7-2 H4-6 c8=5 A6+5 r7=4 H6-7 r4+3 R7+2 k5=6 R7-3 p6+1 C5=4 r4=3 K5=6 r3=5 C4=9 r5-1 R7=4 k6=5 R4-5 r5=4 A5+6 r4=1 R4+3 r1-3 R4=5 r1+3 P1+1 r1-2 R5+3 p9+1",
					MoveNotationType.English)
			};

			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平六 马8进7\r\n2 马二进三 卒7进1\r\n3 车一平二 车9平8\r\n4 车二进四 炮8平9\r\n5 车二平六 马2进3\r\n6 兵七进一 炮2平1\r\n7 马八进七 车1平2\r\n8 兵七进一 卒3进1\r\n9 马七进八 炮1平2\r\n10 马八进九 炮2平1\r\n11 马九进七 炮9平3\r\n12 车九平八 车2进6\r\n13 炮六进七 士6进5\r\n14 炮六退一 炮1平2\r\n15 炮六平八 炮2进5\r\n16 炮八退六 车8进6\r\n17 相七进五 车8平7\r\n18 马三退二 车7平5\r\n19 仕六进五 象7进5\r\n20 兵一进一 卒5进1\r\n21 马二进一 车5退1\r\n22 车六退二 炮3平2\r\n23 帅五平六 车5平2",
					MoveNotationType.SimplifiedChinese,
					"C2=6 h8+7 H2+3 p7+1 R1=2 r9=8 R2+4 c8=9 R2=6 h2+3 P7+1 c2=1 H8+7 r1=2 P7+1 p3+1 H7+8 c1=2 H8+9 c2=1 H9+7 c9=3 R9=8 r2+6 C6+7 a6+5 C6-1 c1=2 C6=8 c2+5 C8-6 r8+6 E7+5 r8=7 H3-2 r7=5 A6+5 e7+5 P1+1 p5+1 H2+1 r5-1 R6-2 c3=2 K5=6 r5=2",
					MoveNotationType.English)
			};
		}
	}

	public static IEnumerable<object[]> MoveTranslatorSimplifiedChineseToUcciTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平五 马8进7\r\n2 马二进三 车9平8\r\n3 车一平二 马2进3\r\n4 兵七进一 卒7进1\r\n5 车二进六 马7进6\r\n6 马八进七 象3进5\r\n7 兵五进一 卒7进1\r\n8 车二平四 马6进7\r\n9 车四平三 马7退5\r\n10 马七进五 卒7进1\r\n11 炮五进二 卒7进1\r\n12 车三平二 车1进1\r\n13 炮五平二 象5退3\r\n14 炮八平五 卒7平6\r\n15 炮五退一 车8进1\r\n16 车九平八 炮8进3\r\n17 车二进二 车1平8\r\n18 车八进七 象7进5\r\n19 车八平七 车8平7\r\n20 炮五进五 士6进5\r\n21 相七进五 车7进5\r\n22 马五进四 车7退2\r\n23 马四退六 炮8平5\r\n24 仕六进五 车7平4\r\n25 马六退七 车4进3\r\n26 车七进二 将5平6\r\n27 车七退三 卒6进1\r\n28 炮五平四 车4平3\r\n29 帅五平六 车3平5\r\n30 炮四平九 车5退1\r\n31 车七平四 将6平5\r\n32 车四退五 车5平4\r\n33 仕五进六 车4平1\r\n34 车四进三 车1退3\r\n35 车四平五 车1进3\r\n36 兵一进一 车1退2\r\n37 车五进三 卒9进1",
					MoveNotationType.SimplifiedChinese,
					"h2e2 h9g7 h0g2 i9h9 i0h0 b9c7 c3c4 g6g5 h0h6 g7f5 b0c2 c9e7 e3e4 g5g4 h6f6 f5g3 f6g6 g3e4 c2e3 g4g3 e2e4 g3g2 g6h6 a9a8 e4h4 e7c9 b2e2 g2f2 e2e1 h9h8 a0b0 h7h4 h6h8 a8h8 b0b7 g9e7 b7c7 h8g8 e1e6 f9e8 c0e2 g8g3 e3f5 g3g5 f5d4 h4e4 d0e1 g5d5 d4c2 d5d2 c7c9 e9f9 c9c6 f2f1 e6f6 d2c2 e0d0 c2e2 f6a6 e2e3 c6f6 f9e9 f6f1 e3d3 e1d2 d3a3 f1f4 a3a6 f4e4 a6a3 i3i4 a3a5 e4e7 i6i5",
					MoveNotationType.UCCI)
			};

			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 炮二平六 马8进7\r\n2 马二进三 卒7进1\r\n3 车一平二 车9平8\r\n4 车二进四 炮8平9\r\n5 车二平六 马2进3\r\n6 兵七进一 炮2平1\r\n7 马八进七 车1平2\r\n8 兵七进一 卒3进1\r\n9 马七进八 炮1平2\r\n10 马八进九 炮2平1\r\n11 马九进七 炮9平3\r\n12 车九平八 车2进6\r\n13 炮六进七 士6进5\r\n14 炮六退一 炮1平2\r\n15 炮六平八 炮2进5\r\n16 炮八退六 车8进6\r\n17 相七进五 车8平7\r\n18 马三退二 车7平5\r\n19 仕六进五 象7进5\r\n20 兵一进一 卒5进1\r\n21 马二进一 车5退1\r\n22 车六退二 炮3平2\r\n23 帅五平六 车5平2",
					MoveNotationType.SimplifiedChinese,
					"h2d2 h9g7 h0g2 g6g5 i0h0 i9h9 h0h4 h7i7 h4d4 b9c7 c3c4 b7a7 b0c2 a9b9 c4c5 c6c5 c2b4 a7b7 b4a6 b7a7 a6c7 i7c7 a0b0 b9b3 d2d9 f9e8 d9d8 a7b7 d8b8 b7b2 b8b2 h9h3 c0e2 h3g3 g2h0 g3e3 d0e1 g9e7 i3i4 e6e5 h0i2 e3e4 d4d2 c7b7 e0d0 e4b4",
					MoveNotationType.UCCI)
			};
		}
	}

	public static IEnumerable<object[]> MoveTranslatorUcciToTraditionalChineseTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 h2e2 h9g7\r\n2 h0g2 i9h9\r\n3 i0h0 b9c7\r\n4 c3c4 g6g5\r\n5 h0h6 g7f5\r\n6 b0c2 c9e7\r\n7 e3e4 g5g4\r\n8 h6f6 f5g3\r\n9 f6g6 g3e4\r\n10 c2e3 g4g3\r\n11 e2e4 g3g2\r\n12 g6h6 a9a8\r\n13 e4h4 e7c9\r\n14 b2e2 g2f2\r\n15 e2e1 h9h8\r\n16 a0b0 h7h4\r\n17 h6h8 a8h8\r\n18 b0b7 g9e7\r\n19 b7c7 h8g8\r\n20 e1e6 f9e8\r\n21 c0e2 g8g3\r\n22 e3f5 g3g5\r\n23 f5d4 h4e4\r\n24 d0e1 g5d5\r\n25 d4c2 d5d2\r\n26 c7c9 e9f9\r\n27 c9c6 f2f1\r\n28 e6f6 d2c2\r\n29 e0d0 c2e2\r\n30 f6a6 e2e3\r\n31 c6f6 f9e9\r\n32 f6f1 e3d3\r\n33 e1d2 d3a3\r\n34 f1f4 a3a6\r\n35 f4e4 a6a3\r\n36 i3i4 a3a5\r\n37 e4e7 i6i5",
					MoveNotationType.UCCI,
					"炮二平五 馬８進７ 馬二進三 車９平８ 車一平二 馬２進３ 兵七進一 卒７進１ 車二進六 馬７進６ 馬八進七 象３進５ 兵五進一 卒７進１ 車二平四 馬６進７ 車四平三 馬７退５ 馬七進五 卒７進１ 炮五進二 卒７進１ 車三平二 車１進１ 炮五平二 象５退３ 炮八平五 卒７平６ 炮五退一 車８進１ 車九平八 炮８進３ 車二進二 車１平８ 車八進七 象７進５ 車八平七 車８平７ 炮五進五 士６進５ 相七進五 車７進５ 馬五進四 車７退２ 馬四退六 炮８平５ 仕六進五 車７平４ 馬六退七 車４進３ 車七進二 將５平６ 車七退三 卒６進１ 炮五平四 車４平３ 帥五平六 車３平５ 炮四平九 車５退１ 車七平四 將６平５ 車四退五 車５平４ 仕五進六 車４平１ 車四進三 車１退３ 車四平五 車１進３ 兵一進一 車１退２ 車五進三 卒９進１",
					MoveNotationType.TraditionalChinese)
			};

			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 h2d2 h9g7\r\n2 h0g2 g6g5\r\n3 i0h0 i9h9\r\n4 h0h4 h7i7\r\n5 h4d4 b9c7\r\n6 c3c4 b7a7\r\n7 b0c2 a9b9\r\n8 c4c5 c6c5\r\n9 c2b4 a7b7\r\n10 b4a6 b7a7\r\n11 a6c7 i7c7\r\n12 a0b0 b9b3\r\n13 d2d9 f9e8\r\n14 d9d8 a7b7\r\n15 d8b8 b7b2\r\n16 b8b2 h9h3\r\n17 c0e2 h3g3\r\n18 g2h0 g3e3\r\n19 d0e1 g9e7\r\n20 i3i4 e6e5\r\n21 h0i2 e3e4\r\n22 d4d2 c7b7\r\n23 e0d0 e4b4",
					MoveNotationType.UCCI,
					"炮二平六 馬８進７ 馬二進三 卒７進１ 車一平二 車９平８ 車二進四 炮８平９ 車二平六 馬２進３ 兵七進一 炮２平１ 馬八進七 車１平２ 兵七進一 卒３進１ 馬七進八 炮１平２ 馬八進九 炮２平１ 馬九進七 炮９平３ 車九平八 車２進６ 炮六進七 士６進５ 炮六退一 炮１平２ 炮六平八 炮２進５ 炮八退六 車８進６ 相七進五 車８平７ 馬三退二 車７平５ 仕六進五 象７進５ 兵一進一 卒５進１ 馬二進一 車５退１ 車六退二 炮３平２ 帥五平六 車５平２",
					MoveNotationType.TraditionalChinese)
			};
		}
	}

	public static IEnumerable<object[]> MoveTranslatorUcciToSimplifiedChineseTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 h2e2 h9g7\r\n2 h0g2 i9h9\r\n3 i0h0 b9c7\r\n4 c3c4 g6g5\r\n5 h0h6 g7f5\r\n6 b0c2 c9e7\r\n7 e3e4 g5g4\r\n8 h6f6 f5g3\r\n9 f6g6 g3e4\r\n10 c2e3 g4g3\r\n11 e2e4 g3g2\r\n12 g6h6 a9a8\r\n13 e4h4 e7c9\r\n14 b2e2 g2f2\r\n15 e2e1 h9h8\r\n16 a0b0 h7h4\r\n17 h6h8 a8h8\r\n18 b0b7 g9e7\r\n19 b7c7 h8g8\r\n20 e1e6 f9e8\r\n21 c0e2 g8g3\r\n22 e3f5 g3g5\r\n23 f5d4 h4e4\r\n24 d0e1 g5d5\r\n25 d4c2 d5d2\r\n26 c7c9 e9f9\r\n27 c9c6 f2f1\r\n28 e6f6 d2c2\r\n29 e0d0 c2e2\r\n30 f6a6 e2e3\r\n31 c6f6 f9e9\r\n32 f6f1 e3d3\r\n33 e1d2 d3a3\r\n34 f1f4 a3a6\r\n35 f4e4 a6a3\r\n36 i3i4 a3a5\r\n37 e4e7 i6i5",
					MoveNotationType.UCCI,
					"炮二平五 马８进７ 马二进三 车９平８ 车一平二 马２进３ 兵七进一 卒７进１ 车二进六 马７进６ 马八进七 象３进５ 兵五进一 卒７进１ 车二平四 马６进７ 车四平三 马７退５ 马七进五 卒７进１ 炮五进二 卒７进１ 车三平二 车１进１ 炮五平二 象５退３ 炮八平五 卒７平６ 炮五退一 车８进１ 车九平八 炮８进３ 车二进二 车１平８ 车八进七 象７进５ 车八平七 车８平７ 炮五进五 士６进５ 相七进五 车７进５ 马五进四 车７退２ 马四退六 炮８平５ 仕六进五 车７平４ 马六退七 车４进３ 车七进二 将５平６ 车七退三 卒６进１ 炮五平四 车４平３ 帅五平六 车３平５ 炮四平九 车５退１ 车七平四 将６平５ 车四退五 车５平４ 仕五进六 车４平１ 车四进三 车１退３ 车四平五 车１进３ 兵一进一 车１退２ 车五进三 卒９进１",
					MoveNotationType.SimplifiedChinese)
			};

			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 h2d2 h9g7\r\n2 h0g2 g6g5\r\n3 i0h0 i9h9\r\n4 h0h4 h7i7\r\n5 h4d4 b9c7\r\n6 c3c4 b7a7\r\n7 b0c2 a9b9\r\n8 c4c5 c6c5\r\n9 c2b4 a7b7\r\n10 b4a6 b7a7\r\n11 a6c7 i7c7\r\n12 a0b0 b9b3\r\n13 d2d9 f9e8\r\n14 d9d8 a7b7\r\n15 d8b8 b7b2\r\n16 b8b2 h9h3\r\n17 c0e2 h3g3\r\n18 g2h0 g3e3\r\n19 d0e1 g9e7\r\n20 i3i4 e6e5\r\n21 h0i2 e3e4\r\n22 d4d2 c7b7\r\n23 e0d0 e4b4",
					MoveNotationType.UCCI,
					"炮二平六 马８进７ 马二进三 卒７进１ 车一平二 车９平８ 车二进四 炮８平９ 车二平六 马２进３ 兵七进一 炮２平１ 马八进七 车１平２ 兵七进一 卒３进１ 马七进八 炮１平２ 马八进九 炮２平１ 马九进七 炮９平３ 车九平八 车２进６ 炮六进七 士６进５ 炮六退一 炮１平２ 炮六平八 炮２进５ 炮八退六 车８进６ 相七进五 车８平７ 马三退二 车７平５ 仕六进五 象７进５ 兵一进一 卒５进１ 马二进一 车５退１ 车六退二 炮３平２ 帅五平六 车５平２",
					MoveNotationType.SimplifiedChinese)
			};
		}
	}

	public static IEnumerable<object[]> MoveTranslatorUcciToEnglishTestData
	{
		get
		{
			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 h2e2 h9g7\r\n2 h0g2 i9h9\r\n3 i0h0 b9c7\r\n4 c3c4 g6g5\r\n5 h0h6 g7f5\r\n6 b0c2 c9e7\r\n7 e3e4 g5g4\r\n8 h6f6 f5g3\r\n9 f6g6 g3e4\r\n10 c2e3 g4g3\r\n11 e2e4 g3g2\r\n12 g6h6 a9a8\r\n13 e4h4 e7c9\r\n14 b2e2 g2f2\r\n15 e2e1 h9h8\r\n16 a0b0 h7h4\r\n17 h6h8 a8h8\r\n18 b0b7 g9e7\r\n19 b7c7 h8g8\r\n20 e1e6 f9e8\r\n21 c0e2 g8g3\r\n22 e3f5 g3g5\r\n23 f5d4 h4e4\r\n24 d0e1 g5d5\r\n25 d4c2 d5d2\r\n26 c7c9 e9f9\r\n27 c9c6 f2f1\r\n28 e6f6 d2c2\r\n29 e0d0 c2e2\r\n30 f6a6 e2e3\r\n31 c6f6 f9e9\r\n32 f6f1 e3d3\r\n33 e1d2 d3a3\r\n34 f1f4 a3a6\r\n35 f4e4 a6a3\r\n36 i3i4 a3a5\r\n37 e4e7 i6i5",
					MoveNotationType.UCCI,
					"C2=5 h8+7 H2+3 r9=8 R1=2 h2+3 P7+1 p7+1 R2+6 h7+6 H8+7 e3+5 P5+1 p7+1 R2=4 h6+7 R4=3 h7-5 H7+5 p7+1 C5+2 p7+1 R3=2 r1+1 C5=2 e5-3 C8=5 p7=6 C5-1 r8+1 R9=8 c8+3 R2+2 r1=8 R8+7 e7+5 R8=7 r8=7 C5+5 a6+5 E7+5 r7+5 H5+4 r7-2 H4-6 c8=5 A6+5 r7=4 H6-7 r4+3 R7+2 k5=6 R7-3 p6+1 C5=4 r4=3 K5=6 r3=5 C4=9 r5-1 R7=4 k6=5 R4-5 r5=4 A5+6 r4=1 R4+3 r1-3 R4=5 r1+3 P1+1 r1-2 R5+3 p9+1",
					MoveNotationType.English)
			};

			yield return new object[]
			{
				new MoveTranslatorTestData(
					"1 h2d2 h9g7\r\n2 h0g2 g6g5\r\n3 i0h0 i9h9\r\n4 h0h4 h7i7\r\n5 h4d4 b9c7\r\n6 c3c4 b7a7\r\n7 b0c2 a9b9\r\n8 c4c5 c6c5\r\n9 c2b4 a7b7\r\n10 b4a6 b7a7\r\n11 a6c7 i7c7\r\n12 a0b0 b9b3\r\n13 d2d9 f9e8\r\n14 d9d8 a7b7\r\n15 d8b8 b7b2\r\n16 b8b2 h9h3\r\n17 c0e2 h3g3\r\n18 g2h0 g3e3\r\n19 d0e1 g9e7\r\n20 i3i4 e6e5\r\n21 h0i2 e3e4\r\n22 d4d2 c7b7\r\n23 e0d0 e4b4",
					MoveNotationType.UCCI,
					"C2=6 h8+7 H2+3 p7+1 R1=2 r9=8 R2+4 c8=9 R2=6 h2+3 P7+1 c2=1 H8+7 r1=2 P7+1 p3+1 H7+8 c1=2 H8+9 c2=1 H9+7 c9=3 R9=8 r2+6 C6+7 a6+5 C6-1 c1=2 C6=8 c2+5 C8-6 r8+6 E7+5 r8=7 H3-2 r7=5 A6+5 e7+5 P1+1 p5+1 H2+1 r5-1 R6-2 c3=2 K5=6 r5=2",
					MoveNotationType.English)
			};
		}
	}

	[Theory]
	[MemberData(nameof(MoveTranslatorEnglishToUcciTestData))]
	[MemberData(nameof(MoveTranslatorEnglishToSimplifiedChineseTestData))]
	[MemberData(nameof(MoveTranslatorEnglishToTraditionalChineseTestData))]

	[MemberData(nameof(MoveTranslatorTraditionalChineseToSimplifiedChineseTestData))]
	[MemberData(nameof(MoveTranslatorTraditionalChineseToEnglishTestData))]
	[MemberData(nameof(MoveTranslatorTraditionalChineseToUcciTestData))]

	[MemberData(nameof(MoveTranslatorSimplifiedChineseToTraditionalChineseTestData))]
	[MemberData(nameof(MoveTranslatorSimplifiedChineseToEnglishTestData))]
	[MemberData(nameof(MoveTranslatorSimplifiedChineseToUcciTestData))]

	[MemberData(nameof(MoveTranslatorUcciToTraditionalChineseTestData))]
	[MemberData(nameof(MoveTranslatorUcciToSimplifiedChineseTestData))]
	[MemberData(nameof(MoveTranslatorUcciToEnglishTestData))]
	public static void ShouldTranslateNotationCorrectly_WhenCallingTranslateTo(MoveTranslatorTestData testData)
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithMoveRecord(testData.OriginalGameRecord, testData.OriginalNotationType)
			.Build();

		var expectedMoves = testData.ExpectedTranslatedGameRecord
			.Split(" ", StringSplitOptions.RemoveEmptyEntries);

		// Act
		// Assert
		Assert.Multiple(() =>
		{
			for (int i = 0; i < game.GetMoveHistory().Count; i++)
			{
				string ucciMove = game.GetMoveHistory()[i].TranslateTo(testData.TargetNotationType);
				Assert.Equal(expectedMoves[i], ucciMove);
			}
		});
	}
}

public record MoveTranslatorTestData(string OriginalGameRecord, MoveNotationType OriginalNotationType, string ExpectedTranslatedGameRecord, MoveNotationType TargetNotationType);
