async function downloadWord(journalId, groupNumber) {
    const response = await fetch(`https://localhost:7059/api/journal/downloadword/${journalId}`, {
        method: 'GET',
    });
    download(response, groupNumber + ".docx");
}

async function downloadPage(journalId, pageId, pageTypeName) {
    const response = await fetch(`https://localhost:7059/api/journal/downloadpage/${journalId}?pageId=${pageId}`, {
        method: 'GET',
    });
    download(response, pageTypeName + ".docx");
}

async function downloadFile(downloadRequest) {
    const response = await fetch(`https://localhost:7059/api/journal/downloaddocument`, {
        method: 'POST',
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(downloadRequest),
    });
    await download(response, downloadRequest.fileName);
}

async function download(response, fileName) {
    if (!response.ok) {
        throw new Error('Failed to download');
    }

    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);

    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();

    link.remove();
    window.URL.revokeObjectURL(url);
}