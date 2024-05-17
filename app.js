const express = require('express');
const app = express();
const amazonScraper = require('amazon-buddy');
const { exec } = require('child_process');
const fs = require('fs');
const path=require('path');
app.use(express.urlencoded({ extended: true }));
app.set('view engine', 'ejs');
app.use(express.static('public'));
app.use(express.static(path.join(__dirname,'public')));
let searchResults = []; // Store search results globally
let storedASIN;

app.get('/', (req, res) => {
  res.render('search');
});


app.get('/redirect', (req, res) => {
  res.render('redirect', { searchResults });
});

app.get('/store-asin', (req, res) => {
  const { asin } = req.query;
  storedASIN = asin; // Store the ASIN in the server-side variable
  const folderName = `asin_response`; // Replace 'your_folder_name' with the actual folder name
  const filePath = `${folderName}/${asin}_response.json`;//asin_response/B0B5D6W912_response.json
 // Dynamic file path based on ASIN B0BHGG9F1Q

  // Check if the JSON file already exists
  fs.access(filePath, fs.constants.F_OK, (err) => {
    if (!err) {
      // The file exists, so read it and render your result page
      fs.readFile(filePath, 'utf8', (readErr, data) => {
        if (readErr) {
          console.error(`Error reading file: ${readErr}`);
          return res.status(500).send('An error occurred during file processing.');
        }
        res.render('final', { path: filePath, data });
      });
    } else {
      // The file doesn't exist, so execute the Python command
      const command = `python review_gen.py ${storedASIN}`;

      // Execute the Python command and wait for it to finish
      exec(command, (error, stdout, stderr) => {
        if (error) {
          console.error(`Error executing Python command: ${error}`);
          return res.status(500).send('An error occurred during processing, and the result file is missing.');
        }

        // Check if the file is now available
        checkFileReady(filePath, (fileReady) => {
          if (fileReady) {
            // Read the file and render your result page
            fs.readFile(filePath, 'utf8', (readErr, data) => {
              if (readErr) {
                console.error(`Error reading file: ${readErr}`);
                return res.status(500).send('An error occurred during file processing.');
              }
              res.render('final', { path: filePath, data });
            });
          } else {
            res.status(500).send('File not ready yet. Please check back later.');
          }
        });
      });
    }
  });
});

function checkFileReady(filePath, callback) {
  // Check if the file exists and has content
  fs.stat(filePath, (err, stats) => {
    if (err) {
      return callback(false);
    }
    if (stats.size > 0) {
      callback(true);
    } else {
      setTimeout(() => {
        checkFileReady(filePath, callback);
      }, 1000); // Check again in 1 second
    }
  });
}

app.post('/search', async (req, res) => {
  try {
    const { keyword } = req.body;
    const result = await amazonScraper.products({ keyword, number: 5 });

    if (Array.isArray(result.result)) {
      searchResults = result.result;
      res.redirect('/redirect');
    } else {
      console.error('Amazon Scraper did not return an array of products:', result);
      res.render('error', { error: 'Amazon Scraper did not return an array of products' });
    }
  } catch (error) {
    console.error('Error while scraping:', error);
    res.render('error', { error });
  }
});

app.listen(4200, () => {
  console.log('Server is running on http://localhost:4200');
});