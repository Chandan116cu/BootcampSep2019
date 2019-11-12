var { questionDetail } = require('../Models/question')
const excelToJson = require('convert-excel-to-json');

const quesFromExcel = async (req, res) => {
    let path = req.file.filename
    const result = excelToJson({
        sourceFile: 'upload/'+path,
        columnToKey: {
            A: 'questionText',
            B: 'option1',
            C: 'option2',
            D: 'option3',
            E: 'option4',
            F: 'answer',
            G: 'weightage',
            H: 'questionImage',
            I: 'answerType'
        }

    })
    console.log('exam code is '+req.body.examCode)
          let i;  
    try {
        //  var arr = result.Sheet2 
        for(i = 0;i<result.Sheet2.length;i++){
            // arr[i].examCode = req.body.examCode
           result.Sheet2[i].examCode = req.body.examCode

        }
         console.log('arr is'+result)
            questionDetail.insertMany(result.Sheet2,(err, docs) => {
            if(err){
                res.status(404).send({msg: 'File uploading failed'})
            }
            else{
                res.status(200).send({ msg: 'Data from excel file saved' });
            }
        })
    } catch (err) {
        console.log(err)
    }
    
}




module.exports = {
    quesFromExcel
}