import { useState } from 'react';

function Counter() {
    const[broj, setBroj] = useState(0);

    return(
        <div>
            <p>Done workouts: {broj}</p>
            <button onClick={() => setBroj(broj+1)}>+1</button>
        </div>
    );
}

export default Counter;