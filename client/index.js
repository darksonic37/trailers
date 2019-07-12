const API_URL = 'http://localhost:5000/api'

async function search(query) {
  try {
    let response = await fetch(`${API_URL}/search/${query}`)
    let json = await response.json()
    return json
  } catch (error) {
    console.error(error)
  }
}

function render(movies) {
  const ul = document.getElementById('movies')

  while(ul.firstChild) {
    ul.removeChild(ul.firstChild)
  }

  for (const movie of movies) {
    if (movie.youtube) {
      const li = document.createElement('li');
      li.innerHTML = `<div><h2>${movie.title}</h2><iframe id="ytplayer-${movie.id}" type="text/html" width="640" height="360" src="${movie.youtube}" frameborder="0"></iframe><p>Release: ${movie.release}</p><p>Rating: ${movie.rating}/10</p>`
      ul.appendChild(li)
    }
  }
}

async function main() {
  // Register autocomplete
  new autoComplete({
    data: {
        src: async () => {
            const query = document.getElementById('autoComplete').value

            if(query) {
              const source = await fetch(`${API_URL}/autocomplete/${query}`)
              const data = await source.json()
              return data
            } else {
              return []
            }
        },
        cache: false,
    },
    maxResults: 5,
    resultsList: {
      render: true,
      container: source => {
        source.setAttribute("id", "autoComplete_results_list");
      },
      destination: document.querySelector("#autoComplete"),
      position: "afterend",
      element: "ul"
    },
    noResults: () => {
      const result = document.createElement("li")
      result.setAttribute("class", "no_result")
      result.setAttribute("tabindex", "1")
      result.innerHTML = "No Results"
      document.querySelector("#autoComplete_results_list").appendChild(result)
    },
    onSelection: feedback => document.querySelector("#autoComplete").value = feedback.selection.value
  })

  // Search on submit
  document.getElementById('submit').addEventListener('click', async function() {
    const query = document.getElementById('autoComplete').value
    const movies = await search(query)
    render(movies)
  })
}

main()
