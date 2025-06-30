
const tableBody = document.getElementById("book-table-body");
let allRows = [];
if (tableBody) {
    allRows = Array.from(tableBody.querySelectorAll("tr"));
}

const galleryItems = document.querySelectorAll("div.gallery-item");
if (galleryItems) {
    allCards = Array.from(galleryItems);
}

const searchbar = document.getElementById("book-searchbar");
const paginationContainer = document.getElementById("pagination-buttons")
const pageSizeWheel = document.getElementById("page-size");
const genreHamburgerMenu = document.getElementById("genre-menu-toggle");
const ownershipHamburgerMenu = document.getElementById("ownership-menu-toggle");
const genreDropdownMenu = document.getElementById("genre-dropdown-menu");
const ownershipDropdownMenu = document.getElementById("dropdown-menu");

const indexCreateButton = document.getElementById("indexCreateButton");
const indexTwocreateButton = document.getElementById("indexTwoCreateButton");
const tableButton = document.getElementById("tableLink");
const cardViewButton = document.getElementById("cardLink");

let rowsPerPage = 5;
let filterGenreRestrictedRows = [...allRows];
let filterOwnershipRestrictedRows = [...allRows];
let filteredRows = [...allRows];
let currentPage = 1;

let filterGenreRestrictedCards = allCards;
let filterOwnershipRestrictedCards = allCards;
let filteredCards = allCards;

let isGenreBurgerRotated = false;
let isOwnershipBurgerRotated = false;
let selectedGenreRadio = null;
let selectedOwnershipRadio = null;


tableButton.addEventListener("click", () => {
    localStorage.setItem('view', 'table');
    window.location.href = ("/Books");
})

cardViewButton.addEventListener("click", () => {
    localStorage.setItem('view', 'card');
    window.location.href = '/Books/Index2';
})

if (galleryItems.length > 0) {
    for (let i = 0; i < galleryItems.length; i++) {
        galleryItems[i].addEventListener("mouseover", () => {
            document.querySelectorAll(".trashButton")[i].classList.remove("d-none");

        })
        galleryItems[i].addEventListener("mouseout", () => {
            document.querySelectorAll(".trashButton")[i].classList.add("d-none");
        })
    }
}


//Keyboard shortcuts
document.addEventListener("keyup", (e) => {
    if (!window.shortcutMap) return; // avoid crash

    if (e.key === shortcutMap.createBook && document.activeElement.id !== "book-searchbar") {
        if (tableBody) {
            indexCreateButton.click();
        } else {
            indexTwocreateButton.click();
        }
    }

    if (e.key === shortcutMap.searchbarFocus && document.activeElement.id === "book-searchbar") {
        searchbar.blur();
        searchbar.value = "";
        if (tableBody) {
            filterRows();
        } else {
            filterCards();
        }
    }
    else if (e.key === shortcutMap.searchbarFocus && document.activeElement.id !== "book-searchbar") {
        searchbar.focus();
        if (!ownershipDropdownMenu.classList.contains("d-none")) {
            ownershipDropdownMenu.classList.add("d-none");
            isOwnershipBurgerRotated = !isOwnershipBurgerRotated;
            ownershipHamburgerMenu.style.transition = 'transform 0.25s ease-in-out';
            ownershipHamburgerMenu.style.transform = isOwnershipBurgerRotated ? 'rotate(90deg)' : 'rotate(0deg)';
        }

        if (!genreDropdownMenu.classList.contains("d-none")) {
            genreDropdownMenu.classList.add("d-none");
            isGenreBurgerRotated = !isGenreBurgerRotated;
            genreHamburgerMenu.style.transition = 'transform 0.25s ease-in-out';
            genreHamburgerMenu.style.transform = isGenreBurgerRotated ? 'rotate(90deg)' : 'rotate(0deg)';
        }
    }

    if (e.key === 'ArrowUp' && (!genreDropdownMenu.classList.contains("d-none") || !ownershipDropdownMenu.classList.contains("d-none"))) {
        filterKeyboardShortcut(-1, "optionUnentered", "optionOwned", "optionOther", "optionFantasy");
    }
    if (e.key === 'ArrowDown' && (!genreDropdownMenu.classList.contains("d-none") || !ownershipDropdownMenu.classList.contains("d-none"))) {
        filterKeyboardShortcut(1, "optionOwned", "optionUnentered", "optionFantasy", "optionOther");
    }

    if (tableBody) {
        if (e.key === 'ArrowRight' && currentPage !== paginationContainer.children.length) {
            currentPage += 1;
            updateTableDisplay();
        }
        if (e.key === 'ArrowLeft' && currentPage !== 1) {
            currentPage -= 1;
            updateTableDisplay();
        }
    }

    if (e.key === shortcutMap.genreFilter && document.activeElement.id !== "book-searchbar") {
        openGenreHamburger();
    }
    if (e.key === shortcutMap.ownershipFilter && document.activeElement.id !== "book-searchbar") {
        openOwnershipHamburger();
    }
})



//Hamburger menu
genreHamburgerMenu.addEventListener("click", openGenreHamburger);
ownershipHamburgerMenu.addEventListener("click", openOwnershipHamburger);


searchbar.addEventListener("click", () => {
    if (!ownershipDropdownMenu.classList.contains("d-none")) {
        ownershipDropdownMenu.classList.add("d-none");
        isOwnershipBurgerRotated = !isOwnershipBurgerRotated;
        ownershipHamburgerMenu.style.transition = 'transform 0.25s ease-in-out';
        ownershipHamburgerMenu.style.transform = isOwnershipBurgerRotated ? 'rotate(90deg)' : 'rotate(0deg)';
    }
    if (!genreDropdownMenu.classList.contains("d-none")) {
        genreDropdownMenu.classList.add("d-none");
        isGenreBurgerRotated = !isGenreBurgerRotated;
        genreHamburgerMenu.style.transition = 'transform 0.25s ease-in-out';
        genreHamburgerMenu.style.transform = isGenreBurgerRotated ? 'rotate(90deg)' : 'rotate(0deg)';
    }
})

if (pageSizeWheel) {
    pageSizeWheel.addEventListener("click", () => {
        if (!ownershipDropdownMenu.classList.contains("d-none")) {
            ownershipDropdownMenu.classList.add("d-none");
            isOwnershipBurgerRotated = !isOwnershipBurgerRotated;
            ownershipHamburgerMenu.style.transition = 'transform 0.25s ease-in-out';
            ownershipHamburgerMenu.style.transform = isOwnershipBurgerRotated ? 'rotate(90deg)' : 'rotate(0deg)';
        }
        if (!genreDropdownMenu.classList.contains("d-none")) {
            genreDropdownMenu.classList.add("d-none");
            isGenreBurgerRotated = !isGenreBurgerRotated;
            genreHamburgerMenu.style.transition = 'transform 0.25s ease-in-out';
            genreHamburgerMenu.style.transform = isGenreBurgerRotated ? 'rotate(90deg)' : 'rotate(0deg)';
        }
    })
}


//dropdown genre menu
document.querySelectorAll('input[type="radio"][name="genreOption"]').forEach(radio => {
    radio.addEventListener('click', function () {

        if (selectedGenreRadio === this) {
            this.checked = false;
            selectedGenreRadio = null;

            if (tableBody) {
                filterGenreRestrictedRows = allRows;
                filterRows();
                updateTableDisplay();
            } else {
                filterGenreRestrictedCards = allCards;
                filterCards();
            }
            return;
        }
        selectedGenreRadio = this;

        if (tableBody) {
            const selectedValue = this.value;
            filterGenreRestrictedRows = allRows.filter(row => {
                return row.dataset.genre === selectedValue;
            });
            filterRows();
            updateTableDisplay();
        } else {
            const selectedValue = this.value;
            filterGenreRestrictedCards = allCards.filter(item => {
                return item.dataset.genre === selectedValue;
            });
            filterCards();
        }
    });
});

//dropdown ownership menu
document.querySelectorAll('input[type="radio"][name="ownershipOption"]').forEach(radio => {
    radio.addEventListener('click', function () {

        if (selectedOwnershipRadio === this) {
            this.checked = false;
            selectedOwnershipRadio = null;

            if (tableBody) {
                filterOwnershipRestrictedRows = allRows;
                filterRows();
                updateTableDisplay();
            } else {
                filterOwnershipRestrictedCards = allCards;
                filterCards();
            }
            return;
        }
        selectedOwnershipRadio = this;
        const selectedValue = this.value;

        if (tableBody) {
            filterOwnershipRestrictedRows = allRows.filter(row => {
                return row.dataset.status === selectedValue;
            });
            filterRows();
            updateTableDisplay();
        } else {
            filterOwnershipRestrictedCards = allCards.filter(item => {
                return item.dataset.status === selectedValue;
            });
            filterCards();
        }
    });
});

//searchbar
if (pageSizeWheel) {
    pageSizeWheel.addEventListener("input", () => {
        if (pageSizeWheel.value != rowsPerPage) {
            rowsPerPage = pageSizeWheel.value;
            updateTableDisplay();
        }
    })
}

//functions with table
function updateTableDisplay() {
    const start = (currentPage - 1) * rowsPerPage;
    const end = start + rowsPerPage;

    allRows.forEach(row => row.classList.add("d-none"));
    filteredRows.slice(start, end).forEach(row => row.classList.remove("d-none"));

    renderPaginationButtons();
}

function renderPaginationButtons() {
    const pageCount = Math.ceil(filteredRows.length / rowsPerPage);
    paginationContainer.innerHTML = "";
    if (pageCount > 1) {
        for (let i = 1; i <= pageCount; i++) {
            const button = document.createElement("button");
            button.className = "btn btn-outline-primary mx-1";
            if (i === currentPage) button.classList.add("active");
            button.innerText = i;
            button.addEventListener("click", () => {
                currentPage = i;
                updateTableDisplay();
            });
            paginationContainer.appendChild(button);
        }
    }
}

function filterRows() {
    const term = searchbar.value.toLowerCase();

    filteredRows = allRows.filter(row => {
        const title = row.dataset.title.toLowerCase() || "";
        const author = row.dataset.author.toLowerCase() || "";
        return (title.includes(term) || author.includes(term)) && filterOwnershipRestrictedRows.includes(row) && filterGenreRestrictedRows.includes(row);
    });

    currentPage = 1;
    updateTableDisplay();
}

//functions with card view

function filterCards() {
    const term = searchbar.value.toLowerCase();

    filteredCards = allCards.filter(item => { 
        const title = item.dataset.title.toLowerCase() || "";
        const author = item.dataset.author.toLowerCase() || "";
        return (title.includes(term) || author.includes(term)) && filterOwnershipRestrictedCards.includes(item) && filterGenreRestrictedCards.includes(item);
    });

    allCards.forEach(item => item.classList.add("d-none"));
    filteredCards.forEach(item => item.classList.remove("d-none"));
}

function openGenreHamburger() {
    if (!ownershipDropdownMenu.classList.contains("d-none")) {
        ownershipDropdownMenu.classList.add("d-none");
        isOwnershipBurgerRotated = !isOwnershipBurgerRotated;
        ownershipHamburgerMenu.style.transition = 'transform 0.25s ease-in-out';
        ownershipHamburgerMenu.style.transform = isOwnershipBurgerRotated ? 'rotate(90deg)' : 'rotate(0deg)';
    }

    genreDropdownMenu.classList.toggle('d-none');
    isGenreBurgerRotated = !isGenreBurgerRotated;
    genreHamburgerMenu.style.transition = 'transform 0.25s ease-in-out';
    genreHamburgerMenu.style.transform = isGenreBurgerRotated ? 'rotate(90deg)' : 'rotate(0deg)';
}

function openOwnershipHamburger() {
    if (!genreDropdownMenu.classList.contains("d-none")) {
        genreDropdownMenu.classList.add("d-none");
        isGenreBurgerRotated = !isGenreBurgerRotated;
        genreHamburgerMenu.style.transition = 'transform 0.25s ease-in-out';
        genreHamburgerMenu.style.transform = isGenreBurgerRotated ? 'rotate(90deg)' : 'rotate(0deg)';
    }

    ownershipDropdownMenu.classList.toggle('d-none');
    isOwnershipBurgerRotated = !isOwnershipBurgerRotated;
    ownershipHamburgerMenu.style.transition = 'transform 0.25s ease-in-out';
    ownershipHamburgerMenu.style.transform = isOwnershipBurgerRotated ? 'rotate(90deg)' : 'rotate(0deg)';
}

function filterKeyboardShortcut(num, firstOwn, lastOwn, firstGen, lastGen) {
    if (!genreDropdownMenu.classList.contains("d-none")) {
        if (selectedGenreRadio == null) {

            selectedGenreRadio = document.getElementById(firstGen);

        } else if (selectedGenreRadio != document.getElementById(lastGen)) {
            for (let i = 1; i <= 15; i++) {
                if (selectedGenreRadio.classList.contains(i.toString())) {
                    let temp = i + num;
                    selectedGenreRadio = document.getElementsByClassName(temp.toString())[0];
                    break;
                }
            }

        }

        selectedGenreRadio.checked = true
        const selectedValue = selectedGenreRadio.value;

        if (tableBody) {
            filterGenreRestrictedRows = allRows.filter(row => {
                return row.dataset.genre === selectedValue;
            });
            filterRows();
            updateTableDisplay();
        } else {
            filterGenreRestricted = galleryItems.filter(item => {
                return item.dataset.genre === selectedValue;
            });
            filterCards();
        }
    }

    if (!ownershipDropdownMenu.classList.contains("d-none")) {
        if (selectedOwnershipRadio == null) {

            selectedOwnershipRadio = document.getElementById(firstOwn);

        } else if (selectedOwnershipRadio != document.getElementById(lastOwn)) {

            for (let i = 21; i <= 23; i++) {
                if (selectedOwnershipRadio.classList.contains(i.toString())) {
                    let temp = i + num;
                    selectedOwnershipRadio = document.getElementsByClassName(temp.toString())[0];
                    break;
                }
            }

        }

        selectedOwnershipRadio.checked = true
        const selectedValue = selectedOwnershipRadio.value;

        if (tableBody) {
            filterGenreRestrictedRows = allRows.filter(row => {
                return row.dataset.status === selectedValue;
            });
            filterRows();
            updateTableDisplay();
        } else {
            filterGenreRestrictedCards = galleryItems.filter(item => {
                return item.dataset.status === selectedValue;
            });
            filterCards();
        }
    }
}


// Initial setup
// Load first page on startup
if (tableBody) {
    searchbar.addEventListener("input", filterRows);
    filterRows(); 
} else {
    searchbar.addEventListener("input", filterCards);
    filterCards();
}