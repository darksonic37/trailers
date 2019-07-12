## Trailers

Movie trailers web application developed at an on-site interview at Dept Agency.

## Development

Run the .NET development server to expose the middleware API on `http://localhost:5000`:
```
cd server/
dotnet clean && dotnet build && dotnet run
```

Run any HTTP server to serve the `client/` directory on `http://localhost:1337`:

```
cd client/
python -m http.server 1337
```
