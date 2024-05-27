using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

// Move JSON strings to JSON files under Resources

public static class ColourManager
{
    //Antigo ja nao funciona, tem nomes diferentes!!!
    private static string xTermPaletteJSON;// = "{ 	\"colours\": 	[ 		{ 			\"index\" : \"0\", 			\"name\" : \"Black(SYSTEM)\", 			\"r\" : \"0\", 			\"g\" : \"0\", 			\"b\" : \"0\" 		}, 		{ 			\"index\" : \"1\", 			\"name\" : \"Maroon(SYSTEM)\", 			\"r\" : \"128\", 			\"g\" : \"0\", 			\"b\" : \"0\" 		}, 		{ 			\"index\" : \"2\", 			\"name\" : \"Green(SYSTEM)\", 			\"r\" : \"0\", 			\"g\" : \"128\", 			\"b\" : \"0\" 		}, 		{ 			\"index\" : \"3\", 			\"name\" : \"Olive(SYSTEM)\", 			\"r\" : \"128\", 			\"g\" : \"128\", 			\"b\" : \"0\" 		}, 		{ 			\"index\" : \"4\", 			\"name\" : \"Navy(SYSTEM)\", 			\"r\" : \"0\", 			\"g\" : \"0\", 			\"b\" : \"127\" 		}, 		{ 			\"index\" : \"5\", 			\"name\" : \"Purple(SYSTEM)\", 			\"r\" : \"128\", 			\"g\" : \"0\", 			\"b\" : \"128\" 		}, 		{ 			\"index\" : \"6\", 			\"name\" : \"Teal(SYSTEM)\", 			\"r\" : \"0\", 			\"g\" : \"128\", 			\"b\" : \"128\" 		}, 		{ 			\"index\" : \"7\", 			\"name\" : \"Silver(SYSTEM)\", 			\"r\" : \"0192\", 			\"g\" : \"192\", 			\"b\" : \"192\" 		}, 		{ 			\"index\" : \"8\", 			\"name\" : \"Gray(SYSTEM)\", 			\"r\" : \"128\", 			\"g\" : \"128\", 			\"b\" : \"128\" 		}, 		{ 			\"index\" : \"9\", 			\"name\" : \"Red(SYSTEM)\", 			\"r\" : \"255\", 			\"g\" : \"0\", 			\"b\" : \"0\" 		}, 		{ 			\"index\" : \"10\", 			\"name\" : \"Lime(SYSTEM)\", 			\"r\" : \"0\", 			\"g\" : \"255\", 			\"b\" : \"0\" 		}, 		{ 			\"index\" : \"11\", 			\"name\" : \"Yellow(SYSTEM)\", 			\"r\" : \"255\", 			\"g\" : \"255\", 			\"b\" : \"0\" 		}, 		{ 			\"index\" : \"12\", 			\"name\" : \"Blue(SYSTEM)\", 			\"r\" : \"0\", 			\"g\" : \"0\", 			\"b\" : \"255\" 		}, 		{ 			\"index\" : \"13\", 			\"name\" : \"Fuchsia(SYSTEM)\", 			\"r\" : \"255\", 			\"g\" : \"0\", 			\"b\" : \"255\" 		}, 		{ 			\"index\" : \"14\", 			\"name\" : \"Aqua(SYSTEM)\", 			\"r\" : \"0\", 			\"g\" : \"255\", 			\"b\" : \"255\" 		}, 		{ 			\"index\" : \"15\", 			\"name\" : \"White(SYSTEM)\", 			\"r\" : \"255\", 			\"g\" : \"255\", 			\"b\" : \"255\" 		} 	] }";
    private static ReferencePalette referencePalette;

    //Antigo ja nao funciona, tem nomes diferentes!!!
    private static string hTMLColoursJSON;// = "{ \"colours\": [  {    \"id\": 1,    \"colourname\": \"maroon\",    \"hexvalue\": \"#800000\"  },  {    \"id\": 2,    \"colourname\": \"darkred\",    \"hexvalue\": \"#8B0000\"  },  {    \"id\": 3,    \"colourname\": \"brown\",    \"hexvalue\": \"#A52A2A\"  },  {    \"id\": 4,    \"colourname\": \"firebrick\",    \"hexvalue\": \"#B22222\"  },  {    \"id\": 5,    \"colourname\": \"crimson\",    \"hexvalue\": \"#DC143C\"  },  {    \"id\": 6,    \"colourname\": \"red\",    \"hexvalue\": \"#FF0000\"  },  {    \"id\": 7,    \"colourname\": \"tomato\",    \"hexvalue\": \"#FF6347\"  },  {    \"id\": 8,    \"colourname\": \"coral\",    \"hexvalue\": \"#FF7F50\"  },  {    \"id\": 9,    \"colourname\": \"indianred\",    \"hexvalue\": \"#CD5C5C\"  },  {    \"id\": 10,    \"colourname\": \"lightcoral\",    \"hexvalue\": \"#F08080\"  },  {    \"id\": 11,    \"colourname\": \"darksalmon\",    \"hexvalue\": \"#E9967A\"  },  {    \"id\": 12,    \"colourname\": \"salmon\",    \"hexvalue\": \"#FA8072\"  },  {    \"id\": 13,    \"colourname\": \"lightsalmon\",    \"hexvalue\": \"#FFA07A\"  },  {    \"id\": 14,    \"colourname\": \"orangered\",    \"hexvalue\": \"#FF4500\"  },  {    \"id\": 15,    \"colourname\": \"darkorange\",    \"hexvalue\": \"#FF8C00\"  },  {    \"id\": 16,    \"colourname\": \"orange\",    \"hexvalue\": \"#FFA500\"  },  {    \"id\": 17,    \"colourname\": \"gold\",    \"hexvalue\": \"#FFD700\"  },  {    \"id\": 18,    \"colourname\": \"darkgoldenrod\",    \"hexvalue\": \"#B8860B\"  },  {    \"id\": 19,    \"colourname\": \"goldenrod\",    \"hexvalue\": \"#DAA520\"  },  {    \"id\": 20,    \"colourname\": \"palegoldenrod\",    \"hexvalue\": \"#EEE8AA\"  },  {    \"id\": 21,    \"colourname\": \"darkkhaki\",    \"hexvalue\": \"#BDB76B\"  },  {    \"id\": 22,    \"colourname\": \"khaki\",    \"hexvalue\": \"#F0E68C\"  },  {    \"id\": 23,    \"colourname\": \"olive\",    \"hexvalue\": \"#808000\"  },  {    \"id\": 24,    \"colourname\": \"yellow\",    \"hexvalue\": \"#FFFF00\"  },  {    \"id\": 25,    \"colourname\": \"yellowgreen\",    \"hexvalue\": \"#9ACD32\"  },  {    \"id\": 26,    \"colourname\": \"darkolivegreen\",    \"hexvalue\": \"#556B2F\"  },  {    \"id\": 27,    \"colourname\": \"olivedrab\",    \"hexvalue\": \"#6B8E23\"  },  {    \"id\": 28,    \"colourname\": \"lawngreen\",    \"hexvalue\": \"#7CFC00\"  },  {    \"id\": 29,    \"colourname\": \"chartreuse\",    \"hexvalue\": \"#7FFF00\"  },  {    \"id\": 30,    \"colourname\": \"greenyellow\",    \"hexvalue\": \"#ADFF2F\"  },  {    \"id\": 31,    \"colourname\": \"darkgreen\",    \"hexvalue\": \"#006400\"  },  {    \"id\": 32,    \"colourname\": \"green\",    \"hexvalue\": \"#008000\"  },  {    \"id\": 33,    \"colourname\": \"forestgreen\",    \"hexvalue\": \"#228B22\"  },  {    \"id\": 34,    \"colourname\": \"lime\",    \"hexvalue\": \"#00FF00\"  },  {    \"id\": 35,    \"colourname\": \"limegreen\",    \"hexvalue\": \"#32CD32\"  },  {    \"id\": 36,    \"colourname\": \"lightgreen\",    \"hexvalue\": \"#90EE90\"  },  {    \"id\": 37,    \"colourname\": \"palegreen\",    \"hexvalue\": \"#98FB98\"  },  {    \"id\": 38,    \"colourname\": \"darkseagreen\",    \"hexvalue\": \"#8FBC8F\"  },  {    \"id\": 39,    \"colourname\": \"mediumspringgreen\",    \"hexvalue\": \"#00FA9A\"  },  {    \"id\": 40,    \"colourname\": \"springgreen\",    \"hexvalue\": \"#00FF7F\"  },  {    \"id\": 41,    \"colourname\": \"seagreen\",    \"hexvalue\": \"#2E8B57\"  },  {    \"id\": 42,    \"colourname\": \"mediumaquamarine\",    \"hexvalue\": \"#66CDAA\"  },  {    \"id\": 43,    \"colourname\": \"mediumseagreen\",    \"hexvalue\": \"#3CB371\"  },  {    \"id\": 44,    \"colourname\": \"lightseagreen\",    \"hexvalue\": \"#20B2AA\"  },  {    \"id\": 45,    \"colourname\": \"darkslategray\",    \"hexvalue\": \"#2F4F4F\"  },  {    \"id\": 46,    \"colourname\": \"teal\",    \"hexvalue\": \"#008080\"  },  {    \"id\": 47,    \"colourname\": \"darkcyan\",    \"hexvalue\": \"#008B8B\"  },  {    \"id\": 48,    \"colourname\": \"aqua\",    \"hexvalue\": \"#00FFFF\"  },  {    \"id\": 49,    \"colourname\": \"cyan\",    \"hexvalue\": \"#00FFFF\"  },  {    \"id\": 50,    \"colourname\": \"lightcyan\",    \"hexvalue\": \"#E0FFFF\"  },  {    \"id\": 51,    \"colourname\": \"darkturquoise\",    \"hexvalue\": \"#00CED1\"  },  {    \"id\": 52,    \"colourname\": \"turquoise\",    \"hexvalue\": \"#40E0D0\"  },  {    \"id\": 53,    \"colourname\": \"mediumturquoise\",    \"hexvalue\": \"#48D1CC\"  },  {    \"id\": 54,    \"colourname\": \"paleturquoise\",    \"hexvalue\": \"#AFEEEE\"  },  {    \"id\": 55,    \"colourname\": \"aquamarine\",    \"hexvalue\": \"#7FFFD4\"  },  {    \"id\": 56,    \"colourname\": \"powderblue\",    \"hexvalue\": \"#B0E0E6\"  },  {    \"id\": 57,    \"colourname\": \"cadetblue\",    \"hexvalue\": \"#5F9EA0\"  },  {    \"id\": 58,    \"colourname\": \"steelblue\",    \"hexvalue\": \"#4682B4\"  },  {    \"id\": 59,    \"colourname\": \"cornflowerblue\",    \"hexvalue\": \"#6495ED\"  },  {    \"id\": 60,    \"colourname\": \"deepskyblue\",    \"hexvalue\": \"#00BFFF\"  },  {    \"id\": 61,    \"colourname\": \"dodgerblue\",    \"hexvalue\": \"#1E90FF\"  },  {    \"id\": 62,    \"colourname\": \"lightblue\",    \"hexvalue\": \"#ADD8E6\"  },  {    \"id\": 63,    \"colourname\": \"skyblue\",    \"hexvalue\": \"#87CEEB\"  },  {    \"id\": 64,    \"colourname\": \"lightskyblue\",    \"hexvalue\": \"#87CEFA\"  },  {    \"id\": 65,    \"colourname\": \"midnightblue\",    \"hexvalue\": \"#191970\"  },  {    \"id\": 66,    \"colourname\": \"navy\",    \"hexvalue\": \"#000080\"  },  {    \"id\": 67,    \"colourname\": \"darkblue\",    \"hexvalue\": \"#00008B\"  },  {    \"id\": 68,    \"colourname\": \"mediumblue\",    \"hexvalue\": \"#0000CD\"  },  {    \"id\": 69,    \"colourname\": \"blue\",    \"hexvalue\": \"#0000FF\"  },  {    \"id\": 70,    \"colourname\": \"royalblue\",    \"hexvalue\": \"#4169E1\"  },  {    \"id\": 71,    \"colourname\": \"blueviolet\",    \"hexvalue\": \"#8A2BE2\"  },  {    \"id\": 72,    \"colourname\": \"indigo\",    \"hexvalue\": \"#4B0082\"  },  {    \"id\": 73,    \"colourname\": \"darkslateblue\",    \"hexvalue\": \"#483D8B\"  },  {    \"id\": 74,    \"colourname\": \"slateblue\",    \"hexvalue\": \"#6A5ACD\"  },  {    \"id\": 75,    \"colourname\": \"mediumslateblue\",    \"hexvalue\": \"#7B68EE\"  },  {    \"id\": 76,    \"colourname\": \"mediumpurple\",    \"hexvalue\": \"#9370DB\"  },  {    \"id\": 77,    \"colourname\": \"darkmagenta\",    \"hexvalue\": \"#8B008B\"  },  {    \"id\": 78,    \"colourname\": \"darkviolet\",    \"hexvalue\": \"#9400D3\"  },  {    \"id\": 79,    \"colourname\": \"darkorchid\",    \"hexvalue\": \"#9932CC\"  },  {    \"id\": 80,    \"colourname\": \"mediumorchid\",    \"hexvalue\": \"#BA55D3\"  },  {    \"id\": 81,    \"colourname\": \"purple\",    \"hexvalue\": \"#800080\"  },  {    \"id\": 82,    \"colourname\": \"thistle\",    \"hexvalue\": \"#D8BFD8\"  },  {    \"id\": 83,    \"colourname\": \"plum\",    \"hexvalue\": \"#DDA0DD\"  },  {    \"id\": 84,    \"colourname\": \"violet\",    \"hexvalue\": \"#EE82EE\"  },  {    \"id\": 85,    \"colourname\": \"fuchsia\",    \"hexvalue\": \"#FF00FF\"  },  {    \"id\": 86,    \"colourname\": \"orchid\",    \"hexvalue\": \"#DA70D6\"  },  {    \"id\": 87,    \"colourname\": \"mediumvioletred\",    \"hexvalue\": \"#C71585\"  },  {    \"id\": 88,    \"colourname\": \"palevioletred\",    \"hexvalue\": \"#DB7093\"  },  {    \"id\": 89,    \"colourname\": \"deeppink\",    \"hexvalue\": \"#FF1493\"  },  {    \"id\": 90,    \"colourname\": \"hotpink\",    \"hexvalue\": \"#FF69B4\"  },  {    \"id\": 91,    \"colourname\": \"lightpink\",    \"hexvalue\": \"#FFB6C1\"  },  {    \"id\": 92,    \"colourname\": \"pink\",    \"hexvalue\": \"#FFC0CB\"  },  {    \"id\": 93,    \"colourname\": \"antiquewhite\",    \"hexvalue\": \"#FAEBD7\"  },  {    \"id\": 94,    \"colourname\": \"beige\",    \"hexvalue\": \"#F5F5DC\"  },  {    \"id\": 95,    \"colourname\": \"bisque\",    \"hexvalue\": \"#FFE4C4\"  },  {    \"id\": 96,    \"colourname\": \"blanchedalmond\",    \"hexvalue\": \"#FFEBCD\"  },  {    \"id\": 97,    \"colourname\": \"wheat\",    \"hexvalue\": \"#F5DEB3\"  },  {    \"id\": 98,    \"colourname\": \"cornsilk\",    \"hexvalue\": \"#FFF8DC\"  },  {    \"id\": 99,    \"colourname\": \"lemonchiffon\",    \"hexvalue\": \"#FFFACD\"  },  {    \"id\": 100,    \"colourname\": \"lightgoldenrodyellow\",    \"hexvalue\": \"#FAFAD2\"  },  {    \"id\": 101,    \"colourname\": \"lightyellow\",    \"hexvalue\": \"#FFFFE0\"  },  {    \"id\": 102,    \"colourname\": \"saddlebrown\",    \"hexvalue\": \"#8B4513\"  },  {    \"id\": 103,    \"colourname\": \"sienna\",    \"hexvalue\": \"#A0522D\"  },  {    \"id\": 104,    \"colourname\": \"chocolate\",    \"hexvalue\": \"#D2691E\"  },  {    \"id\": 105,    \"colourname\": \"peru\",    \"hexvalue\": \"#CD853F\"  },  {    \"id\": 106,    \"colourname\": \"sandybrown\",    \"hexvalue\": \"#F4A460\"  },  {    \"id\": 107,    \"colourname\": \"burlywood\",    \"hexvalue\": \"#DEB887\"  },  {    \"id\": 108,    \"colourname\": \"tan\",    \"hexvalue\": \"#D2B48C\"  },  {    \"id\": 109,    \"colourname\": \"rosybrown\",    \"hexvalue\": \"#BC8F8F\"  },  {    \"id\": 110,    \"colourname\": \"moccasin\",    \"hexvalue\": \"#FFE4B5\"  },  {    \"id\": 111,    \"colourname\": \"navajowhite\",    \"hexvalue\": \"#FFDEAD\"  },  {    \"id\": 112,    \"colourname\": \"peachpuff\",    \"hexvalue\": \"#FFDAB9\"  },  {    \"id\": 113,    \"colourname\": \"mistyrose\",    \"hexvalue\": \"#FFE4E1\"  },  {    \"id\": 114,    \"colourname\": \"lavenderblush\",    \"hexvalue\": \"#FFF0F5\"  },  {    \"id\": 115,    \"colourname\": \"linen\",    \"hexvalue\": \"#FAF0E6\"  },  {    \"id\": 116,    \"colourname\": \"oldlace\",    \"hexvalue\": \"#FDF5E6\"  },  {    \"id\": 117,    \"colourname\": \"papayawhip\",    \"hexvalue\": \"#FFEFD5\"  },  {    \"id\": 118,    \"colourname\": \"seashell\",    \"hexvalue\": \"#FFF5EE\"  },  {    \"id\": 119,    \"colourname\": \"mintcream\",    \"hexvalue\": \"#F5FFFA\"  },  {    \"id\": 120,    \"colourname\": \"slategray\",    \"hexvalue\": \"#708090\"  },  {    \"id\": 121,    \"colourname\": \"lightslategray\",    \"hexvalue\": \"#778899\"  },  {    \"id\": 122,    \"colourname\": \"lightsteelblue\",    \"hexvalue\": \"#B0C4DE\"  },  {    \"id\": 123,    \"colourname\": \"lavender\",    \"hexvalue\": \"#E6E6FA\"  },  {    \"id\": 124,    \"colourname\": \"floralwhite\",    \"hexvalue\": \"#FFFAF0\"  },  {    \"id\": 125,    \"colourname\": \"aliceblue\",    \"hexvalue\": \"#F0F8FF\"  },  {    \"id\": 126,    \"colourname\": \"ghostwhite\",    \"hexvalue\": \"#F8F8FF\"  },  {    \"id\": 127,    \"colourname\": \"honeydew\",    \"hexvalue\": \"#F0FFF0\"  },  {    \"id\": 128,    \"colourname\": \"ivory\",    \"hexvalue\": \"#FFFFF0\"  },  {    \"id\": 129,    \"colourname\": \"azure\",    \"hexvalue\": \"#F0FFFF\"  },  {    \"id\": 130,    \"colourname\": \"snow\",    \"hexvalue\": \"#FFFAFA\"  },  {    \"id\": 131,    \"colourname\": \"black\",    \"hexvalue\": \"#000000\"  },  {    \"id\": 132,    \"colourname\": \"dimgray\",    \"hexvalue\": \"#696969\"  },  {    \"id\": 133,    \"colourname\": \"gray\",    \"hexvalue\": \"#808080\"  },  {    \"id\": 134,    \"colourname\": \"darkgray\",    \"hexvalue\": \"#A9A9A9\"  },  {    \"id\": 135,    \"colourname\": \"silver\",    \"hexvalue\": \"#C0C0C0\"  },  {    \"id\": 136,    \"colourname\": \"lightgray\",    \"hexvalue\": \"#D3D3D3\"  },  {    \"id\": 137,    \"colourname\": \"gainsboro\",    \"hexvalue\": \"#DCDCDC\"  },  {    \"id\": 138,    \"colourname\": \"whitesmoke\",    \"hexvalue\": \"#F5F5F5\"  },  {    \"id\": 139,    \"colourname\": \"white\",    \"hexvalue\": \"#FFFFFF\"  }]}";
    private static HTMLColours htmlColours;

    private static string variationsJSON;
    private static ColVariations colVariations;

    private static string packsJSON;
    private static ColPacks Packs;

    private static string colorNamesJSON;
    private static ColorNames ColorNames;


    //Antigo ja nao funciona, tem nomes diferentes!!!
    private static string correspondencesJSON;// = "{     \"black\":  {   \"bright\": \"0\",   \"normal\": \"0\",   \"dark\": \"0\"     },     \"white\":  {   \"bright\": \"15\",   \"normal\": \"15\",   \"dark\": \"15\"  },  \"primary\":  {   \"bright\": \"14\",   \"normal\": \"12\",   \"dark\": \"4\"     },     \"secondary\":  {   \"bright\": \"11\",   \"normal\": \"9\",   \"dark\": \"1\"     } } ";
    private static ColourCorrespondences correspondences;

    //Antigo ja nao funciona, tem nomes diferentes!!!
    private static string colourSetsJSON;// = "{ \"couloursets\": 	[       { 			\"name\" : \"EX0101\",           \"variations\" : 			{ 				\"bright\" : 				{ 					\"r\" : \"212\",                    \"g\" : \"195\",                    \"b\" : \"182\" 				}, 				\"normal\" : 				{ \"r\" : \"190\", 					\"g\" : \"164\", 					\"b\" : \"146\"                 }, 				\"dark\" : 				{ \"r\" : \"169\", 					\"g\" : \"134\", 					\"b\" : \"109\"                 } 			} 		}, 		{ \"name\" : \"EX0102\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"243\", 					\"g\" : \"221\", 					\"b\" : \"206\"                 }, 				\"normal\" : 				{ \"r\" : \"236\", 					\"g\" : \"205\", 					\"b\" : \"182\"                 }, 				\"dark\" : 				{ \"r\" : \"224\", 					\"g\" : \"171\", 					\"b\" : \"133\"                 } } }, 		{ \"name\" : \"EX0103\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"255\", 					\"g\" : \"229\", 					\"b\" : \"223\"                 }, 				\"normal\" : 				{ \"r\" : \"225\", 					\"g\" : \"203\", 					\"b\" : \"199\"                 }, 				\"dark\" : 				{ \"r\" : \"229\", 					\"g\" : \"168\", 					\"b\" : \"165\"                 } } }, 		{ \"name\" : \"EX0104\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"189\", 					\"g\" : \"163\", 					\"b\" : \"196\"                 }, 				\"normal\" : 				{ \"r\" : \"173\", 					\"g\" : \"140\", 					\"b\" : \"181\"                 }, 				\"dark\" : 				{ \"r\" : \"130\", 					\"g\" : \"105\", 					\"b\" : \"136\"                 } } }, 		{ \"name\" : \"EX0105\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"188\", 					\"g\" : \"185\", 					\"b\" : \"214\"                 }, 				\"normal\" : 				{ \"r\" : \"154\", 					\"g\" : \"150\", 					\"b\" : \"193\"                 }, 				\"dark\" : 				{ \"r\" : \"120\", 					\"g\" : \"115\", 					\"b\" : \"173\"                } } }, 		{ \"name\" : \"EX0106\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"169\", 					\"g\" : \"186\", 					\"b\" : \"203\"                 }, 				\"normal\" : 				{ \"r\" : \"111\", 					\"g\" : \"140\", 					\"b\" : \"168\"                 }, 				\"dark\" : 				{ \"r\" : \"83\", 					\"g\" : \"105\", 					\"b\" : \"126\"                 } } }, 		{ \"name\" : \"EX0107\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"177\", 					\"g\" : \"195\", 					\"b\" : \"195\"                 }, 				\"normal\" : 				{ \"r\" : \"138\", 					\"g\" : \"165\", 					\"b\" : \"164\"                 }, 				\"dark\" : 				{ \"r\" : \"99\", 					\"g\" : \"135\", 					\"b\" : \"134\"                 } } }, 		{ \"name\" : \"EX0108\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"199\", 					\"g\" : \"206\", 					\"b\" : \"185\"                 }, 				\"normal\" : 				{ \"r\" : \"162\", 					\"g\" : \"173\", 					\"b\" : \"138\"                 }, 				\"dark\" : 				{ \"r\" : \"122\", 					\"g\" : \"130\", 					\"b\" : \"104\"                 } } }, 		{ \"name\" : \"EX0109\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"240\", 					\"g\" : \"240\", 					\"b\" : \"238\"                 }, 				\"normal\" : 				{ \"r\" : \"225\", 					\"g\" : \"225\", 					\"b\" : \"222\"                 }, 				\"dark\" : 				{ \"r\" : \"211\", 					\"g\" : \"211\", 					\"b\" : \"205\"                 } } }, 		{ \"name\" : \"EX0110\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"211\", 					\"g\" : \"211\", 					\"b\" : \"205\"                 }, 				\"normal\" : 				{ \"r\" : \"181\", 					\"g\" : \"181\", 					\"b\" : \"172\"                 }, 				\"dark\" : 				{ \"r\" : \"136\", 					\"g\" : \"136\", 					\"b\" : \"129\"                 } } }, 		{ \"name\" : \"EX0111\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"104\", 					\"g\" : \"104\", 					\"b\" : \"99\"              }, 				\"normal\" : 				{ \"r\" : \"91\", 					\"g\" : \"91\", 					\"b\" : \"86\"              }, 				\"dark\" : 				{ \"r\" : \"79\", 					\"g\" : \"79\", 					\"b\" : \"75\"              } } }, 		{ \"name\" : \"ST01\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"210\", 					\"g\" : \"245\", 					\"b\" : \"249\"                 }, 				\"normal\" : 				{ \"r\" : \"178\", 					\"g\" : \"229\", 					\"b\" : \"237\"                 }, 				\"dark\" : 				{ \"r\" : \"110\", 					\"g\" : \"179\", 					\"b\" : \"186\"                 } } }, 		{ \"name\" : \"ST02\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"255\", 					\"g\" : \"250\", 					\"b\" : \"222\"                 }, 				\"normal\" : 				{ \"r\" : \"234\", 					\"g\" : \"228\", 					\"b\" : \"178\"                 }, 				\"dark\" : 				{ \"r\" : \"193\", 					\"g\" : \"182\", 					\"b\" : \"116\"                 } } }, 		{ \"name\" : \"ST03\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"217\", 					\"g\" : \"255\", 					\"b\" : \"221\"                 }, 				\"normal\" : 				{ \"r\" : \"179\", 					\"g\" : \"232\", 					\"b\" : \"187\"                 }, 				\"dark\" : 				{ \"r\" : \"124\", 					\"g\" : \"191\", 					\"b\" : \"132\"                 } } }, 		{ \"name\" : \"C01\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"255\", 					\"g\" : \"255\", 					\"b\" : \"255\"                 }, 				\"normal\" : 				{ \"r\" : \"255\", 					\"g\" : \"255\", 					\"b\" : \"255\"                 }, 				\"dark\" : 				{ \"r\" : \"255\", 					\"g\" : \"255\", 					\"b\" : \"255\"                 } } }, 		{ \"name\" : \"C10\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"0\", 					\"g\" : \"255\", 					\"b\" : \"255\"                 }, 				\"normal\" : 				{ \"r\" : \"0\", 					\"g\" : \"0\", 					\"b\" : \"255\"                 }, 				\"dark\" : 				{ \"r\" : \"0\", 					\"g\" : \"0\", 					\"b\" : \"80\"              } } }, 		{ \"name\" : \"C11\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"255\", 					\"g\" : \"255\", 					\"b\" : \"0\"               }, 				\"normal\" : 				{ \"r\" : \"255\", 					\"g\" : \"0\", 					\"b\" : \"0\"               }, 				\"dark\" : 				{ \"r\" : \"80\", 					\"g\" : \"0\", 					\"b\" : \"0\"               } } }, 		{ \"name\" : \"C00\", 			\"variations\" : 			{ \"bright\" : 				{ \"r\" : \"0\", 					\"g\" : \"0\", 					\"b\" : \"0\"               }, 				\"normal\" : 				{ \"r\" : \"0\", 					\"g\" : \"0\", 					\"b\" : \"0\"               }, 				\"dark\" : 				{ \"r\" : \"0\", 					\"g\" : \"0\", 					\"b\" : \"0\"               } } } 	] }";
    public static Colours colourSets;

    private static string _usercolorsjson;
    public static string UserColorsJSON
    {
        get => _usercolorsjson;
        set
        {
            _usercolorsjson = value;
            userColors = JsonUtility.FromJson<UserColorsArray>(_usercolorsjson);
            if (userColors.All.Count() > 0)
            {
                Load_SVG_From_File.PrimaryCode = userColors.All[0].PrimaryColor;
                Load_SVG_From_File.SecondaryCode = userColors.All[0].SecondaryColor;
            }
            else
            {
                Load_SVG_From_File.PrimaryCode = Load_SVG_From_File.PrimaryCodeDefault;
                Load_SVG_From_File.SecondaryCode = Load_SVG_From_File.SecondaryCodeDefault;
                userColors.All = new UserColors[1] { new UserColors(Load_SVG_From_File.PrimaryCodeDefault, Load_SVG_From_File.SecondaryCodeDefault) };
                Debug.Log("Created Instance");
            }
            Debug.Log("User colors Updated " + Load_SVG_From_File.PrimaryCode + " " + Load_SVG_From_File.SecondaryCode);
            //AvatarReader.ReloadAvatar();
        }

    }

    public static string XTermPaletteJSON
    {
        get => xTermPaletteJSON; set
        {
            xTermPaletteJSON = value;
            referencePalette = JsonUtility.FromJson<ReferencePalette>(XTermPaletteJSON);

        }
    }

    public static string HTMLColoursJSON
    {
        get => hTMLColoursJSON; set
        {
            hTMLColoursJSON = value;

            htmlColours = JsonUtility.FromJson<HTMLColours>(HTMLColoursJSON);
        }
    }

    public static string VariationsJSON
    {
        get => variationsJSON; set
        {
            variationsJSON = value;

            colVariations = JsonUtility.FromJson<ColVariations>(VariationsJSON);
        }
    }

    public static string PacksJSON
    {
        get => packsJSON; set
        {
            packsJSON = value;
            Packs = JsonUtility.FromJson<ColPacks>(PacksJSON);
        }
    }

    public static string ColorNamesJSON
    {
        get => colorNamesJSON; set
        {
            colorNamesJSON = value;
            ColorNames = JsonUtility.FromJson<ColorNames>(ColorNamesJSON);
        }
    }

    public static string CorrespondencesJSON
    {
        get => correspondencesJSON; set
        {
            correspondencesJSON = value;
            correspondences = JsonUtility.FromJson<ColourCorrespondences>(CorrespondencesJSON);
        }
    }

    public static string ColourSetsJSON
    {
        get => colourSetsJSON; set
        {
            colourSetsJSON = value;
            colourSets = JsonUtility.FromJson<Colours>(ColourSetsJSON);

        }
    }

    public static UserColorsArray userColors;

    static ColourManager()
    {
        //GeneratePalettes();
    }

    public static bool CheckUnlocked(Coulourset Cor)
    {
        if (UserSetsComponent.AllUserSets.All.Where(x => x.SetID == Cor.SetCode).Count() > 0 || Cor.SetCode == "Starter")
        {
            if (Cor.code.StartsWith("C0") || Cor.code.StartsWith("C1"))
                return false;
            else
                return true;
        }
        else
        {

            //  Debug.Log("Color " + Cor.code + "  locked! " + Cor.SetCode);
            return false;
        }
    }


    public static string ReplaceColors(string svg, string primaryPack, string secondaryPack)
    {
        //Debug.Log("Current Colors " + primaryPack + " " + secondaryPack);
        string primaryBrightSource = HexEncode(referencePalette.colours[correspondences.color.Where(x => x.code == "C10" && x.variation == 0).First().xtermindex].red, referencePalette.colours[correspondences.color.Where(x => x.code == "C10" && x.variation == 0).First().xtermindex].green, referencePalette.colours[correspondences.color.Where(x => x.code == "C10" && x.variation == 0).First().xtermindex].blue);
        string primaryNormalSource = HexEncode(referencePalette.colours[correspondences.color.Where(x => x.code == "C10" && x.variation == 1).First().xtermindex].red, referencePalette.colours[correspondences.color.Where(x => x.code == "C10" && x.variation == 1).First().xtermindex].green, referencePalette.colours[correspondences.color.Where(x => x.code == "C10" && x.variation == 1).First().xtermindex].blue);
        //Os svgs tem a cor azul escura mal, reduzi aqui o blue por 1 so para dar
        string primaryDarkSource = HexEncode(referencePalette.colours[correspondences.color.Where(x => x.code == "C10" && x.variation == 2).First().xtermindex].red, referencePalette.colours[correspondences.color.Where(x => x.code == "C10" && x.variation == 2).First().xtermindex].green, referencePalette.colours[correspondences.color.Where(x => x.code == "C10" && x.variation == 2).First().xtermindex].blue - 1);

        string secondaryBrightSource = HexEncode(referencePalette.colours[correspondences.color.Where(x => x.code == "C11" && x.variation == 0).First().xtermindex].red, referencePalette.colours[correspondences.color.Where(x => x.code == "C11" && x.variation == 0).First().xtermindex].green, referencePalette.colours[correspondences.color.Where(x => x.code == "C11" && x.variation == 0).First().xtermindex].blue);
        string secondaryNormalSource = HexEncode(referencePalette.colours[correspondences.color.Where(x => x.code == "C11" && x.variation == 1).First().xtermindex].red, referencePalette.colours[correspondences.color.Where(x => x.code == "C11" && x.variation == 1).First().xtermindex].green, referencePalette.colours[correspondences.color.Where(x => x.code == "C11" && x.variation == 1).First().xtermindex].blue);
        string secondaryDarkSource = HexEncode(referencePalette.colours[correspondences.color.Where(x => x.code == "C11" && x.variation == 2).First().xtermindex].red, referencePalette.colours[correspondences.color.Where(x => x.code == "C11" && x.variation == 2).First().xtermindex].green, referencePalette.colours[correspondences.color.Where(x => x.code == "C11" && x.variation == 2).First().xtermindex].blue);



        string blackSource = HexEncode(referencePalette.colours[correspondences.color.Where(x => x.code == "C00").First().xtermindex].red, referencePalette.colours[correspondences.color.Where(x => x.code == "C00").First().xtermindex].green, referencePalette.colours[correspondences.color.Where(x => x.code == "C00").First().xtermindex].blue);
        string whiteSource = HexEncode(referencePalette.colours[correspondences.color.Where(x => x.code == "C01").First().xtermindex].red, referencePalette.colours[correspondences.color.Where(x => x.code == "C01").First().xtermindex].green, referencePalette.colours[correspondences.color.Where(x => x.code == "C01").First().xtermindex].blue);

        //Define a cor que vai substituir pela propria ao inicio
        string primaryBrightTarget = primaryBrightSource;
        string primaryNormalTarget = primaryNormalSource;
        string primaryDarkTarget = primaryDarkSource;

        string secondaryBrightTarget = secondaryBrightSource;
        string secondaryNormalTarget = secondaryNormalSource;
        string secondaryDarkTarget = secondaryDarkSource;

        //------------


        // Transform colour names into hex values
        foreach (HTMLColour colour in htmlColours.colours)
            svg = svg.Replace(colour.name, colour.hexvalue);

        foreach (Coulourset set in colourSets.couloursets)
        {
            if (set.code == primaryPack)
            {
                if (set.variation == 0)
                    primaryBrightTarget = HexEncode(set.red, set.green, set.blue);
                if (set.variation == 1)
                    primaryNormalTarget = HexEncode(set.red, set.green, set.blue);
                if (set.variation == 2)
                    primaryDarkTarget = HexEncode(set.red, set.green, set.blue);
            }

            if (set.code == secondaryPack)
            {
                if (set.variation == 0)
                    secondaryBrightTarget = HexEncode(set.red, set.green, set.blue);
                if (set.variation == 1)
                    secondaryNormalTarget = HexEncode(set.red, set.green, set.blue);
                if (set.variation == 2)
                    secondaryDarkTarget = HexEncode(set.red, set.green, set.blue);

            }
        }
        string returnSVG;
        //Fecha os olhos
        // Uppercase
        returnSVG = svg.Replace(primaryBrightSource, primaryBrightTarget);
        returnSVG = returnSVG.Replace(primaryNormalSource, primaryNormalTarget);
        returnSVG = returnSVG.Replace(primaryDarkSource, primaryDarkTarget);





        returnSVG = returnSVG.Replace(secondaryBrightSource, secondaryBrightTarget);


        returnSVG = returnSVG.Replace(secondaryNormalSource, secondaryNormalTarget);


        returnSVG = returnSVG.Replace(secondaryDarkSource, secondaryDarkTarget);




        //Lowercase
        returnSVG = returnSVG.Replace(primaryBrightSource.ToLower(), primaryBrightTarget);

        returnSVG = returnSVG.Replace(primaryNormalSource.ToLower(), primaryNormalTarget);

        returnSVG = returnSVG.Replace(primaryDarkSource.ToLower(), primaryDarkTarget);




        if (CanTheHexCodeBeShortned(secondaryBrightSource))
            returnSVG = returnSVG.Replace(HexCodeShortned(secondaryBrightSource).ToLower(), secondaryDarkTarget);
        else
            returnSVG = returnSVG.Replace(secondaryBrightSource.ToLower(), secondaryBrightTarget);


        returnSVG = returnSVG.Replace(secondaryNormalSource.ToLower(), secondaryNormalTarget);


        returnSVG = returnSVG.Replace(secondaryDarkSource.ToLower(), secondaryDarkTarget);

        //Ja podes abrir

        // Debug.Log(returnSVG);
        return returnSVG;
    }

    private static string HexEncode(int r, int g, int b)
    {
        return "#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
    }

    private static bool CanTheHexCodeBeShortned(string Hexcode)
    {
        //Debug.Log(Hexcode);
        if (Hexcode == "#FFFF00" || Hexcode == "#ffff00")
            return true;
        else
            return false;
    }
    private static string HexCodeShortned(string Hexcode)
    {
        //Debug.Log("toma la " + "#" + Hexcode[1].ToString() + Hexcode[3].ToString() + Hexcode[5].ToString());
        if (Hexcode == "#FFFF00" || Hexcode == "#ffff00")
            return "#" + Hexcode[1].ToString() + Hexcode[3].ToString() + Hexcode[5].ToString();
        else
            return Hexcode;
    }

}
