import React, { useState } from 'react';

function Files() {
  const [file, setFile] = useState(null);
  const [status, setStatus] = useState('');
  const [filename, setFilename] = useState('');

  const handleFileChange = (e) => {
    setFile(e.target.files[0]);
    setStatus('');
  };

  const handleUpload = async () => {
    if (!file) {
      setStatus('No file selected');
      return;
    }

    const formData = new FormData();
    formData.append('file', file);

    try {
      // Replace with your actual backend endpoint
      const response = await fetch('/api/fileUpload', {
        method: 'POST',
        body: formData,
      });

      if (response.ok) {
        setStatus('File uploaded successfully!');
      } else {
        setStatus('Upload failed.');
      }
    } catch (error) {
      console.error('Error:', error);
      setStatus('Upload failed (network error).');
    }
  };

  const downloadFile = async () => {
    if (!filename) {
      setStatus('Please enter a filename.');
      return;
    }

    try {
      const response = await fetch(`api/fileDownload/${filename}`);
      if (!response.ok) {
        setStatus('File not found or error downloading.');
        return;
      }

      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);

      const a = document.createElement('a');
      a.href = url;
      a.download = filename; // <-- use filename here
      document.body.appendChild(a);
      a.click();
      a.remove();
      window.URL.revokeObjectURL(url);

      setStatus('Download started.');
    } catch (error) {
      console.error('Download error:', error);
      setStatus('Download failed (network error).');
    }
  };

  return (
    <div>
      <div>
        <h2>File Upload</h2>
        <input type="file" onChange={handleFileChange} />
        <button onClick={handleUpload}>Upload</button>
        {status && <p>{status}</p>}
      </div>
      <br></br>
      <br></br>
      <div>
        <h2>File Download</h2>
        <input type="text" value={filename} onChange={(e) => setFilename(e.target.value)} placeholder="Enter filename"></input>
        <button onClick={downloadFile}>Download</button>
        {status && <p>{status}</p>}
      </div>
    </div>

  );
}

export default Files;
