const express = require('express');
const multer = require('multer');
const path = require('path');
const fs = require('fs');

const uploadDir = '/uploads';       // Mount this in Docker
const app = express();

// Store files in /uploads (make sure it exists in the container)
const storage = multer.diskStorage({
    destination: uploadDir,
    filename: (req, file, cb) => {
        cb(null, file.originalname)  // Save using original filename
    }
});

const upload = multer({ storage });

app.post('/upload', upload.single('file'), (req, res) => {
    console.log('Received file: req.file.originalname');
    res.status(200).send('File stored');
});

app.get('/uploads/:filename', (req, res) => {
    const filename = req.params.filename;
    const filePath = path.join(uploadDir, filename);

    fs.access(filePath, fs.constants.F_OK, (err) =>{
        if (err){
            return res.status(404).send('File not found');
        }

        res.download(filePath, filename, (err) => {
            if (err){
                console.error('Error sending file:', err);
                res.status(500).send('Error sending file.');
            }
        });
    });
});

app.listen(8080, () => {
    console.log('File server running on port 8080');
});