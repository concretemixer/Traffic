<? 
header('Content-Type: application/json; charset=utf-8'); 

require("common.php");

$data = (object)array( "top" => (object)array ( ) ); 

$user_id = $_GET["user_id"];
$db = @mysqli_connect("localhost", $db_user, $db_pass, $db_name);


$sql = "SELECT user, COUNT( state ) AS levels, SUM( score ) score_ttl FROM  `progress` WHERE state >=3 GROUP BY user ORDER BY levels DESC , score_ttl DESC";


$result = @mysqli_query($db, $sql);

$a = array();


$a=0;
while (true)
{
    $row = @mysqli_fetch_assoc($result);
    if ($row==null)
	break;
    $a++;
    if ($a<=10 || $row['user']==$user_id) {
        $data->top->$a = (object)array( 
		'user' => $row['user'], 
		'levels' => $row['levels'],
		'score' => $row['score_ttl'],
		); 
    }

//	    $a[] = array('id' => (int)$row["poll_option_id"], 
//		"text" => $row["poll_option_text"]);
}

/*
for ($a=0;$a<27;$a++) { 
$data->levels->$a = (object)array( 
'score' => '0', 
'state' => '2', 
); 
} 
  */
mysqli_close($db);

//https://api.vk.com/method/users.get?user_ids=4677342,183273

$ids = array();
foreach($data->top as $val) {
//    $val->name="Vasya";
$ids[] = $val->user;
}

$result = file_get_contents('https://api.vk.com/method/users.get?lang=0&v=5.67&user_ids='.implode(",",$ids));
//echo $result."\n";
$names = json_decode($result);
//echo json_encode($names)."\n";

$a=0;
foreach($data->top as $val) {
    if ($val->user==$names->response[$a]->id)
        $val->name=$names->response[$a]->first_name." ".$names->response[$a]->last_name;
     else
        $val->name = "??";
    $a++;
}

$json = json_encode($data, JSON_UNESCAPED_UNICODE); 
echo $json; 
?>