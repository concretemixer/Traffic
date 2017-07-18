<? 
header('Content-Type: application/json'); 

require("common.php");

$data = (object)array( "levels" => (object)array ( ) ); 

$user_id = $_GET["user_id"];
$db = @mysqli_connect("localhost", $db_user, $db_pass, $db_name);

mysqli_autocommit($db, FALSE);

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


$sql = 'SELECT *,NOW() as n FROM tries WHERE user='.$user_id;
$result = @mysqli_query($db, $sql);

$row = @mysqli_fetch_assoc($result);
if ($row==null) {
    $data->tries = 30;
    $data->diff = 3600;
}
else {
    $data->tries = $row['tries'];

    $to_time = strtotime($row['last_try']);
    $from_time = strtotime($row['n']);

    $diff = abs($from_time - $to_time);
    $data->diff = $diff;

    if ($data->tries<=0) {
        $data->tries = intval($diff / (60*5)) * 5;
        if ($data->tries>30)
            $data->tries = 30;

        $sql = "UPDATE `tries` SET tries=".$data->tries.",last_try=NOW() WHERE user=$user_id";
        @mysqli_query($db, $sql);
    }
}


mysqli_commit($db);
mysqli_close($db);

$json = json_encode($data); 
echo $json; 
?>