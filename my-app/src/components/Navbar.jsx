import './Navbar.css';

function Navbar(){
    return(
        <nav className="navbar">
            <span className="logo">Workout Tracker</span>
            <ul className="nav-links">
                <li><a status="active" href="#Home">Home</a></li>
                <li><a href="#Workouts">Workouts</a></li>
                <li><a href="#Statistics">Statistics</a></li>
                <li className="rightA"><a href="#AboutUs">About us</a></li>
            </ul>
            </nav>
            );
            }
export default Navbar;