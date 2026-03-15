using DeltaLitmus.Systems.Core.SceneSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class LitmusRunner : Game
{
    private SpriteBatch _spriteBatch;
    private const float FixedTimeStep = 1f / 60f;
    private float _accumulator = 0f;

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (dt > 0.25f) dt = 0.25f;
        _accumulator += dt;

        while (_accumulator >= FixedTimeStep)
        {
            SceneManager.FixedUpdate(FixedTimeStep);
            _accumulator -= FixedTimeStep;
        }

        SceneManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        float alpha = _accumulator / FixedTimeStep;

        SceneManager.Draw(gameTime, _spriteBatch, alpha);

        base.Draw(gameTime);
    }
}