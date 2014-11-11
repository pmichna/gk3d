using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace gk3d
{
    class Arena : Cuboid
    {
        public Cuboid LeftPost { get; private set; }
        public Cuboid RightPost { get; private set; }
        public Cuboid Net { get; private set; }
        public Model Bench1;
        private Model bench2;
        private ContentManager _content;

        public Arena(ContentManager content, Vector3 center, int width, int height, int depth, Color color)
            : base(center, width, height, depth, true, color)
        {
            _content = content;
            LeftPost = new Cuboid(center + new Vector3(-width * 0.2f, -height / 4f, 0), 5, height / 2, 2, false , Color.White);
            RightPost = new Cuboid(center + new Vector3(width * 0.2f, -height / 4f, 0), 5, height / 2, 2, false, Color.White);
            Net = new Cuboid(center + new Vector3(0, -height / 8f, 0), (int) (RightPost.Center.X - LeftPost.Center.X), height/4, 1, false, Color.Beige);
            Bench1 = content.Load<Model>("bench");

        }
    }
}
