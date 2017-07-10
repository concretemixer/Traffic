<?php

require("common.php");

$phpbb_root_path = (defined('PHPBB_ROOT_PATH')) ? PHPBB_ROOT_PATH : '../';
$phpEx = "php";

define('IN_PHPBB', true);

$phpbb_root_path = (defined('PHPBB_ROOT_PATH')) ? PHPBB_ROOT_PATH : '../';
$phpEx = "php";
require($phpbb_root_path . 'includes/template.' . $phpEx);
require($phpbb_root_path . 'includes/session.' . $phpEx);
require($phpbb_root_path . 'includes/auth.' . $phpEx);
require($phpbb_root_path . 'includes/functions.' . $phpEx);
require($phpbb_root_path . 'includes/functions_content.' . $phpEx);

$table_prefix = "forum";
require($phpbb_root_path . 'includes/constants.' . $phpEx);
require($phpbb_root_path . 'includes/db/' . "mysqli" . '.' . $phpEx);
require($phpbb_root_path . 'includes/utf/utf_tools.' . $phpEx);

$question_id = $_GET["id"];

$db = new dbal_mysqli();

$db->sql_connect("localhost", $db_user, $db_pass, $db_name, 3306, false, false);

require('get_my_id.' . $phpEx);

header('Content-type: text/json; charset=UTF-8');
header('Cache-Control: private, no-cache="set-cookie"');
header('Expires: 0');
header('Pragma: no-cache');

{
    $sql = 'SELECT * FROM phpbb_poll_options WHERE topic_id='.$question_id;
}

$result = $db->sql_query($sql);

$a = array();


while ($row = $db->sql_fetchrow($result))
{
	    $a[] = array('id' => (int)$row["poll_option_id"], 
		"text" => $row["poll_option_text"]);
    
}

$questions = array('options' => $a);

echo json_encode($questions, JSON_PRETTY_PRINT );


?>