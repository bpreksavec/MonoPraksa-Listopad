import './App.css';
import Button from './components/Button';
import Navbar from './components/Navbar';

function Header(){
  return(
    <header>
      <h1>Workout Tracker</h1>
    </header>
  );
}

function Description(){
      const name = "Workout Tracker";
      const year = 2026;      
  return(
    <div>
      <p className = "description">{name} is web application for logging workouts developed in {year}.</p>
    </div>
  );
}

function Footer(){
  return(
    <p className="footer">© 2026</p>
  );
}

function App() {
  return(
  <div className="page">
    <Navbar />
    <main>
      <Description />
      <div className="groupButtons">
        <Button text="Dodaj trening" />
        <Button text="Spremi" />
        <Button text="Obriši" />
      </div>
    </main>
    <Footer />
  </div>
  );
}

export default App;