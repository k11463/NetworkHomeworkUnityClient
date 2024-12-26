package WebService

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"

	// GORM
	"github.com/glebarez/sqlite"
	"gorm.io/gorm"
)

type GameRecord struct {
	UserName string `json:"username"`
	Score    uint   `json:"score"`
}

var GameDB *gorm.DB
var GameRecordAry []GameRecord
var GameRecordAryLen int32

func HttpGetHighestRecord(w http.ResponseWriter, r *http.Request) {
	switch r.Method {
	case "GET":
		emp := GameRecord{
			UserName: "PlayerOne",
			Score:    100,
		}
		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(emp)
	default:
		fmt.Fprintln(w, "Sorry, only GET method are supported.")
	}
}

func HttpGetTop10Records(w http.ResponseWriter, r *http.Request) {
	switch r.Method {
	case "GET":
		w.Header().Set("Content-Type", "application/json")
		top10Records, bOK := GetTop10GameRecord()
		if bOK {
			json.NewEncoder(w).Encode(top10Records)
		} else {
			w.Header().Set("Content-Type", "application/json")
			json.NewEncoder(w).Encode("Error, can't get top records.")
		}
	default:
		fmt.Fprintln(w, "Sorry, only GET method are supported.")
	}
}

func HttpUploadRecord(w http.ResponseWriter, r *http.Request) {
	switch r.Method {
	case "POST":
		var record GameRecord
		// Try to decode the request body into the struct. If there is an error,
		// respond to the client with the error message and a 400 status code.
		err := json.NewDecoder(r.Body).Decode(&record)
		if err != nil {
			http.Error(w, err.Error(), http.StatusBadRequest)
			return
		}

		AddGameRecord(record)

		w.Header().Set("Content-Type", "application/json")
		top10Records, bOK := GetTop10GameRecord()
		if bOK {
			json.NewEncoder(w).Encode(top10Records)
		} else {
			w.Header().Set("Content-Type", "application/json")
			json.NewEncoder(w).Encode("Error, can't get top records.")
		}
	default:
		fmt.Fprintln(w, "Sorry, only GET method are supported.")
	}
}

func StartWebService() {
	LoadGameDB()
	http.HandleFunc("/Record", HttpUploadRecord)
	http.HandleFunc("/Records", HttpGetTop10Records)

	if err := http.ListenAndServe(":8080", nil); err != nil {
		log.Fatalln(err)
	}
}

func LoadGameDB() {
	var err error
	GameDB, err = gorm.Open(sqlite.Open("MiniGamDB.db"), &gorm.Config{})
	fmt.Printf("LoadGameDB")
	if err != nil {
		log.Fatalln(err)
	}

	// get GameRecord from db.
	res := GameDB.Find(&GameRecordAry)
	GameRecordAryLen = int32(res.RowsAffected)

	fmt.Printf(" Done\n")
}

func AddGameRecord(record GameRecord) {
	if GameDB != nil {
		result := GameDB.Create(&record) // pass pointer
		// INSERT INTO `game_records` (`user_name`,`score`) VALUES ({userName}, {score})

		if result.Error != nil {
			fmt.Println(result.Error)
		}
	}
}

func AddGameRecordToDB(userName string, score int32) {
	if GameDB != nil {
		record := GameRecord{UserName: userName, Score: uint(score)}
		result := GameDB.Create(&record) // pass pointer
		// INSERT INTO `game_records` (`user_name`,`score`) VALUES ({userName}, {score})

		if result.Error != nil {
			fmt.Println(result.Error)
		}
	}
}

func GetTop10GameRecord() (records []GameRecord, bOK bool) {
	bOK = false

	if GameDB != nil {
		// SQL: SELECT `id`, `name` FROM `users` LIMIT 9
		if err := GameDB.Model(&GameRecord{}).Order("score desc").Limit(10).Find(&records).Error; err == nil {
			bOK = true
		}
	}

	return
}

func FindUserHighestGameRecord(userName string) (bOK bool, score int32) {
	bOK = false
	score = 0

	if GameDB != nil {
		var record GameRecord

		if err := GameDB.Where("user_name = ?", userName).Order("score desc").First(&record).Error; err == nil {
			bOK = true
			score = int32(record.Score)
		}
	}

	return
}
