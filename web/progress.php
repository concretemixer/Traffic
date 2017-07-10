<? 
header('Content-Type: application/json'); 

require("common.php");

$data = (object)array( "levels" => (object)array ( ) ); 

$user_id = $_GET["user_id"];
$db = @mysqli_connect("localhost", $db_user, $db_pass, $db_name);


$sql = 'SELECT * FROM progress WHERE user='.$user_id;


$result = @mysqli_query($db, $sql);

$a = array();


while (true)
{
    $row = @mysqli_fetch_assoc($result);
    if ($row==null)
	break;
    $a = $row["level"];
    $data->levels->$a = (object)array( 
		'score' => $row['score'], 
		'state' => $row['state'],
		'attempts' => $row['attempts'],
		); 

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

$json = json_encode($data); 
echo $json; 
?>