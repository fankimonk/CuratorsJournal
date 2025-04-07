async function downloadWord(journalId, groupNumber) {
    const response = await fetch(`https://localhost:7059/api/journal/downloadword/${journalId}`, {
        method: 'GET',
    });

    if (!response.ok) {
        throw new Error('Failed to download');
    }

    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);

    const link = document.createElement('a');
    link.href = url;
    link.download = groupNumber + ".docx";
    document.body.appendChild(link);
    link.click();

    link.remove();
    window.URL.revokeObjectURL(url);
}