const express = require('express')
const app = express()
const { Users } = require('../controller')
const { Ques } = require('../controller')
const middleware = require("../auth/middleware");
const jwt = require('jsonwebtoken');
const { SECRET } = require("../config/config")


const createToken = require("../auth/authenticator").checkAuth;

module.exports = () => {
    app.post('/login', async(req, res) => {
        const result = await createToken(req)
        if (result.token == "null") {
            res.status(400).send(result)
        } else {
            res.status(200).send(result)
        }

    })

    app.post('/signup', async(req, res) => {
            const result = await Users.userRecord(req, res)
            res.send(result)
        })
        //for viewing the details of loggedin user
    app.get('/loggedIn', async(req, res) => {
        const response = await Users.loggedInDetails(req, res)
        res.send(response)
    })

    //examiner will create exam details
    app.post('/exam', (req, res) => {
            Users.examDetail(req, res)
        })
        //examiner will view exam
    app.get('/exam', (req, res) => {
            Users.viewExamDetail(req, res)
        })
        //examiner will edit test details
    app.patch('/exam', (req, res) => {
            res.send({ "data": req.body })
        })
        //examiner will delete test using test id
    app.delete('/exam/:id', (req, res) => {
            res.send({ "data": req.body })
        })
        //examiner will view performance of candidates
    app.get('/exam/performance', (req, res) => {
        console.log('yes')
        const response = Users.studentPerformance(req, res)
        return response
    })

    //examiner will write exam questions
    app.post('/exam/question', (req, res) => {
            Users.question(req, res)
        })
        //examiner will views questions 
    app.get('/exam/question/:id', (req, res) => {
            Users.getQuestionDetail(req, res)
        })
        //examiner will edit questions
    app.patch('/exam/question/:id', (req, res) => {
            res.send({ "data": req.body })
        })
        //candidates will view quesions using accesskey
    app.get('/test', middleware, async(req, res) => {
        const response = await Ques.testQuestions(req, res)
        return response
    })

    //post answers selected by candidates
    app.post('/test', middleware, async(req, res) => {
        const response = await Ques.saveCandidateAnswers(req, res)
        return response
    })

    app.post('/test/assessKey', async(req, res) => {
        const response = await Ques.checkAccessKey(req, res)
        return response
    })

    app.patch('/examiner', async(req, res) => {

        const result = await Users.facultyUpd(req, res)
        res.send(result)
    })

    //admin will add examiner
    app.post('/examiner', (req, res) => {
        const response = adminDetail.adminDetails(req, res)
        return response;
    })

    //admin will view examiner
    app.get('/examiner', async(req, res) => {
        const result = await Users.fetchData(req, res)
        res.send(result);
    })

    //admin will delete examiner using id of examiner
    app.delete('/examiner/:id', (req, res) => {
        const result = Users.facultyDel(req, res)
        res.send(result)
    })

    //admin will view test created by each examiner using their id
    app.get('/examiner/:id', async(req, res) => {
        const result = await Users.testDetails(req, res)
        res.send(result);
    })

    //examiner will create test details
    app.post('/exam', (req, res) => {
        Users.examDetail(req, res)
    })

    //examiner will view test
    app.get('/exam', async(req, res) => {
        const result = await Users.userDetails(req, res)
        res.send(result)
    })

    //examiner will edit test details
    app.patch('/exam', (req, res) => {
        res.send({ "data": req.body })
    })

    //examiner will delete test using test id
    app.delete('/exam/:id', (req, res) => {
        res.send({ "data": req.body })
    })

    //examiner will view performance of candidates
    app.get('/exam/performance', (req, res) => {
        res.send("Hello Word")
    })

    //examiner will write tests questions
    app.post('/exam/question', (req, res) => {
            // res.send({"data":req.body})
            Users.question(req, res)
        })
        //examiner will views questions 
    app.get('/exam/question', (req, res) => {
            res.send("hello world")
        })
        //examiner will edit questions
    app.patch('/exam/question/:id', (req, res) => {
        res.send({ "data": req.body })
    })

    //examiner will delete question by id
    app.delete('/exam/question/:id', (req, res) => {
        res.send({ "data": req.body })
    })

    app.delete('/exam/question/:id', (req, res) => {
        Users.question(req, res)
    })

    return app
}