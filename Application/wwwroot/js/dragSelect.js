export function createSelectionBox(tableRef, startX, startY) {
    removeSelectionBox(); // Remove any existing selection box

    const table = document.querySelector(`[_bl_${tableRef.id}]`);
    if (!table) return;

    const tableRect = table.getBoundingClientRect();
    const box = document.createElement('div');
    box.id = 'selection-box';
    box.className = 'selection-box';

    box.style.left = `${startX - tableRect.left}px`;
    box.style.top = `${startY - tableRect.top}px`;
    box.style.width = '0px';
    box.style.height = '0px';

    table.appendChild(box);
}

export function updateSelectionBox(tableRef, startX, startY, currentX, currentY) {
    const table = document.querySelector(`[_bl_${tableRef.id}]`);
    const box = document.getElementById('selection-box');
    if (!table || !box) return;

    const tableRect = table.getBoundingClientRect();
    const left = Math.min(startX, currentX) - tableRect.left;
    const top = Math.min(startY, currentY) - tableRect.top;
    const width = Math.abs(currentX - startX);
    const height = Math.abs(currentY - startY);

    box.style.left = `${left}px`;
    box.style.top = `${top}px`;
    box.style.width = `${width}px`;
    box.style.height = `${height}px`;
}

export function removeSelectionBox() {
    const box = document.getElementById('selection-box');
    if (box) box.remove();
}

export function getSelectedCells(tableRef) {
    const table = document.querySelector(`[_bl_${tableRef.id}]`);
    const box = document.getElementById('selection-box');
    if (!table || !box) return [];

    const boxRect = box.getBoundingClientRect();
    const cells = table.querySelectorAll('td');
    const selectedIds = [];

    cells.forEach(cell => {
        const cellRect = cell.getBoundingClientRect();
        if (!(cellRect.right < boxRect.left ||
            cellRect.left > boxRect.right ||
            cellRect.bottom < boxRect.top ||
            cellRect.top > boxRect.bottom)) {
            selectedIds.push(cell.getAttribute('key'));
        }
    });

    return selectedIds;
}