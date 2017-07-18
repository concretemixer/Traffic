<? 
//header('Content-Type: application/json'); 

require("common.php");

$data = (object)array( "levels" => (object)array ( 
"0" => (object)array( 
'score' => '0', 
'state' => '0', 
), 
) 
); 

$user_id = $_GET["user_id"];
$level = $_GET["level"];

$db = @mysqli_connect("localhost", $db_user, $db_pass, $db_name);

mysqli_autocommit($db, FALSE);

$sql = "SELECT * FROM progress WHERE user=$user_id and level=$level";

$result = @mysqli_query($db, $sql);

if (mysqli_num_rows($result)==0) {
    $sql = "INSERT INTO `progress`(`user`, `level`, `state`, `score`, `attempts`) VALUES ($user_id,$level,0,0,1)";
}
else {
    $sql = "UPDATE `progress` SET attempts=attempts+1 WHERE user=$user_id and level=$level";
}

$sql = "SELECT *,NOW() as n FROM tries WHERE user=$user_id";

$result = @mysqli_query($db, $sql);

if (mysqli_num_rows($result)==0) {
    $sql = "INSERT INTO `tries`(`user`, `tries`, `last_try`) VALUES ($user_id,30,NOW())";
}
else {
    $row = @mysqli_fetch_assoc($result);

    $to_time = strtotime($row['last_try']);
    $from_time = strtotime($row['n']);

    $diff = abs($from_time - $to_time);
    $tries = $row['tries'];

    if ($tries<=0) {
        $tries = intval($diff / (60*5)) * 5;
        if ($tries>30)
            $tries = 30;
        $tries--;
        $sql = "UPDATE `tries` SET tries=$tries,last_try=NOW() WHERE user=$user_id";
    }
    else {
        if ($level!=0)
            $sql = "UPDATE `tries` SET tries=tries-1,last_try=NOW() WHERE user=$user_id";
    }
}


@mysqli_query($db, $sql);
mysqli_commit($db);
  
mysqli_close($db);

echo "OK";

//$json = json_encode($data); 
//echo $json; 
?>