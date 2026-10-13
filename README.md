# CampusEats Lab 11

React/Vite campus menu with search, category filters and a pickup planning list. ASP.NET Core 8 serves `/api/menu` and `/health`. Without a database connection the API explicitly uses seed data. Without a reachable API the frontend labels its menu as demo data. The app does not submit orders or accept payments.

## Local development

`npm install`, `npm run build`, then `npm run preview`.

Run the API with `dotnet run --project backend`. Set `VITE_API_URL=http://localhost:8080` in an ignored `.env` file and use `npm run dev`. Run frontend on port 5173; set `FrontendOrigins` to the actual frontend origin if different.

## Vercel

Import this GitHub repository. Framework Vite, build `npm run build`, output `dist`, root repository directory. Configure `VITE_API_URL` with the live HTTPS API URL and redeploy. A placeholder is allowed by Lab 11 but does not connect the three tiers.

## Render backend

Create a Docker web service from this repository with root directory `backend`. Dockerfile `Dockerfile`. Set `FrontendOrigins` to the exact live frontend origin without a trailing slash. The API listens on `0.0.0.0` using `PORT` or 8080. Health endpoint `/health`. Configure `ConnectionStrings__Default` through the dashboard; never commit credentials. Test `/api/menu` after deployment.

## PostgreSQL

Create Neon or Supabase PostgreSQL. Set the backend connection using `Host=<host>;Database=<database>;Username=<user>;Password=<secret>;SSL Mode=Require`. Use the hosting provider's certificate settings; do not disable certificate verification. Apply the committed migration from a trusted local terminal with `ConnectionStrings__Default` configured in that session:

`dotnet tool install --global dotnet-ef --version 8.0.11`

`dotnet ef database update --project backend`

Authentication is outside this minimal deployment app. If JWT authentication is added, configure `Jwt__Key`, `Jwt__Issuer`, and `Jwt__Audience` through the host; never expose JWT secrets via Vite variables.

## Monitoring

Check GitHub Actions CI and Vercel build logs. The API logs request method, path and response status. Verify frontend data source label, `/api/menu` HTTP 200, exact-origin CORS preflight, database migration status and HTTPS URLs. Screenshots must hide secret values.
