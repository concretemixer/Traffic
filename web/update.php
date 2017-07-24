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
$score = $_GET["score"];
$state = $_GET["state"];


$db = @mysqli_connect("localhost", $db_user, $db_pass, $db_name);

if ($score>30000)
    $score=0;

$sql = "SELECT * FROM progress WHERE user=$user_id and level=$level";

$ip = $_SERVER['REMOTE_ADDR'];
$result = @mysqli_query($db, $sql);

if (mysqli_num_rows($result)==0) {
    $sql = "INSERT INTO progress(user, level, state, score, attempts) VALUES ($user_id,$level,$state,$score,0)";
	echo $sql; 
}
else {
    $sql = "UPDATE progress SET state=$state,score=$score WHERE user=$user_id and level=$level";
echo $sql; 
}

mysqli_query($db, $sql);

  
mysqli_close($db);

echo "OK"; 
?>