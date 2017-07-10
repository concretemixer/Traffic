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


$sql = "SELECT * FROM progress WHERE user=$user_id and level=$level";

$result = @mysqli_query($db, $sql);

if (mysqli_num_rows($result)==0) {
    $sql = "INSERT INTO `progress`(`user`, `level`, `state`, `score`, `attempts`) VALUES ($user_id,$level,0,0,1)";
}
else {
    $sql = "UPDATE `progress` SET attempts=attempts+1 WHERE user=$user_id and level=$level";
}

@mysqli_query($db, $sql);

  
mysqli_close($db);

echo "OK";

//$json = json_encode($data); 
//echo $json; 
?>